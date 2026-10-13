using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Tests;

/// <summary>
/// Unit tests for PricingService — no HTTP, tests business rules directly.
/// </summary>
public class PricingServiceTests
{
    private readonly AppDbContext _context;
    private readonly PricingService _service;

    public PricingServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("PricingTestDb_" + Guid.NewGuid())
            .Options;
        _context = new AppDbContext(options);
        _service = new PricingService(_context);

        // Seed a test coupon
        _context.Coupons.AddRange(
            new Coupon
            {
                Code = "SAVE10", Description = "10% off", MinOrderValue = 500,
                PercentDiscount = 10, MaxDiscountAmount = 100, FreeShipping = false,
                IsActive = true, StartsOn = DateTime.UtcNow.AddDays(-1), EndsOn = DateTime.UtcNow.AddDays(30),
                CreatedOn = DateTime.UtcNow, TotalUses = 0
            },
            new Coupon
            {
                Code = "FLAT50", Description = "Flat ₹50", MinOrderValue = 200,
                FixedAmount = 50, FreeShipping = false,
                IsActive = true, StartsOn = DateTime.UtcNow.AddDays(-1), EndsOn = DateTime.UtcNow.AddDays(30),
                CreatedOn = DateTime.UtcNow, TotalUses = 0
            },
            new Coupon
            {
                Code = "FREESHIP", Description = "Free ship", MinOrderValue = 0,
                FreeShipping = true, IsActive = true,
                StartsOn = DateTime.UtcNow.AddDays(-1), EndsOn = DateTime.UtcNow.AddDays(30),
                CreatedOn = DateTime.UtcNow, TotalUses = 0
            },
            new Coupon
            {
                Code = "EXPIRED", Description = "Expired", MinOrderValue = 0,
                FixedAmount = 50, IsActive = true,
                StartsOn = DateTime.UtcNow.AddDays(-60), EndsOn = DateTime.UtcNow.AddDays(-1),
                CreatedOn = DateTime.UtcNow, TotalUses = 0
            }
        );
        _context.SaveChanges();
    }

    // ── GST ───────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(1000, 180)]
    [InlineData(500, 90)]
    [InlineData(100, 18)]
    [InlineData(0, 0)]
    public void CalculateTax_Returns18Percent(decimal subtotal, decimal expected)
    {
        var tax = _service.CalculateTax(subtotal);
        Assert.Equal(expected, tax);
    }

    // ── Shipping ──────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(998, 49)]   // below threshold
    [InlineData(999, 0)]    // exactly at threshold → free
    [InlineData(1000, 0)]   // above threshold
    public void CalculateShipping_FreeAbove999(decimal subtotal, decimal expected)
    {
        var shipping = _service.CalculateShipping(subtotal);
        Assert.Equal(expected, shipping);
    }

    // ── Coupon: percentage ────────────────────────────────────────────────────

    [Fact]
    public void CalculateDiscount_PercentCoupon_AppliesCorrectly()
    {
        // 10% of 1000 = 100, which equals the max, so discount = 100
        var discount = _service.CalculateDiscount(1000, "SAVE10");
        Assert.Equal(100m, discount);
    }

    [Fact]
    public void CalculateDiscount_PercentCoupon_RespectsMaxCap()
    {
        // 10% of 2000 = 200, but max is 100
        var discount = _service.CalculateDiscount(2000, "SAVE10");
        Assert.Equal(100m, discount);
    }

    [Fact]
    public void CalculateDiscount_PercentCoupon_BelowMinOrder_ReturnsZero()
    {
        // SAVE10 requires min 500
        var discount = _service.CalculateDiscount(499, "SAVE10");
        Assert.Equal(0m, discount);
    }

    // ── Coupon: fixed amount ──────────────────────────────────────────────────

    [Fact]
    public void CalculateDiscount_FixedCoupon_Returns50()
    {
        var discount = _service.CalculateDiscount(500, "FLAT50");
        Assert.Equal(50m, discount);
    }

    [Fact]
    public void CalculateDiscount_FixedCoupon_BelowMin_ReturnsZero()
    {
        // FLAT50 requires min 200
        var discount = _service.CalculateDiscount(150, "FLAT50");
        Assert.Equal(0m, discount);
    }

    // ── Coupon: free shipping ─────────────────────────────────────────────────

    [Fact]
    public void ValidateCoupon_FreeShipping_MarksShippingFree()
    {
        var result = _service.ValidateCoupon("FREESHIP", 100);
        Assert.True(result.Valid);
        Assert.True(result.FreeShipping);
    }

    // ── Coupon: edge cases ────────────────────────────────────────────────────

    [Fact]
    public void CalculateDiscount_InvalidCode_ReturnsZero()
    {
        var discount = _service.CalculateDiscount(1000, "BADCODE");
        Assert.Equal(0m, discount);
    }

    [Fact]
    public void CalculateDiscount_ExpiredCoupon_ReturnsZero()
    {
        var discount = _service.CalculateDiscount(1000, "EXPIRED");
        Assert.Equal(0m, discount);
    }

    [Fact]
    public void CalculateDiscount_NullCode_ReturnsZero()
    {
        var discount = _service.CalculateDiscount(1000, null);
        Assert.Equal(0m, discount);
    }

    // ── Total calculation ─────────────────────────────────────────────────────

    [Fact]
    public void CalculateTotal_CombinesAllComponents()
    {
        var total = _service.CalculateTotal(1000, 50, 49, 180);
        // 1000 - 50 + 49 + 180 = 1179
        Assert.Equal(1179m, total);
    }

    [Fact]
    public void CalculateTotal_FreeShipping_CorrectTotal()
    {
        var subtotal = 1000m;
        var discount = 0m;
        var shipping = 0m; // free shipping
        var tax = _service.CalculateTax(subtotal); // 180

        var total = _service.CalculateTotal(subtotal, discount, shipping, tax);
        Assert.Equal(1180m, total);
    }
}
