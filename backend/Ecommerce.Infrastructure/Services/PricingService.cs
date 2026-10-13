using Ecommerce.Core.Entities;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Services;

public class PricingService : IPricingService
{
    private readonly AppDbContext _context;

    public PricingService(AppDbContext context)
    {
        _context = context;
    }

    public decimal CalculateTax(decimal subtotal)
    {
        return Math.Round(subtotal * 0.18m, 2);
    }

    public decimal CalculateShipping(decimal subtotal)
    {
        return subtotal >= 999 ? 0 : 49;
    }

    public decimal CalculateDiscount(decimal subtotal, string? couponCode)
    {
        if (string.IsNullOrWhiteSpace(couponCode))
            return 0;

        var coupon = _context.Coupons
            .FirstOrDefault(c => c.Code == couponCode.ToUpper() && c.IsActive);

        if (coupon == null)
            return 0;

        var now = DateTime.Now;
        if (now < coupon.StartsOn || (coupon.EndsOn.HasValue && now > coupon.EndsOn.Value))
            return 0;

        if (subtotal < coupon.MinOrderValue)
            return 0;

        decimal discount = 0;
        if (coupon.FixedAmount.HasValue)
        {
            discount = coupon.FixedAmount.Value;
        }
        else if (coupon.PercentDiscount.HasValue)
        {
            discount = Math.Round(subtotal * coupon.PercentDiscount.Value / 100m, 2);
            if (coupon.MaxDiscountAmount > 0 && discount > coupon.MaxDiscountAmount)
                discount = coupon.MaxDiscountAmount;
        }

        if (discount > subtotal)
            discount = subtotal;

        return discount;
    }

    public decimal CalculateTotal(decimal subtotal, decimal discount, decimal shipping, decimal tax)
    {
        return subtotal - discount + shipping + tax;
    }

    public CouponResult ValidateCoupon(string? couponCode, decimal subtotal)
    {
        if (string.IsNullOrWhiteSpace(couponCode))
        {
            return new CouponResult { Valid = false, Message = "Enter a coupon code." };
        }

        var coupon = _context.Coupons
            .FirstOrDefault(c => c.Code == couponCode.ToUpper() && c.IsActive);

        if (coupon == null)
        {
            return new CouponResult { Valid = false, Code = couponCode, Message = "That coupon code is not valid." };
        }

        var now = DateTime.Now;
        if (now < coupon.StartsOn)
        {
            return new CouponResult { Valid = false, Code = coupon.Code, Message = $"{coupon.Code} is not active yet. Check the validity window." };
        }

        if (coupon.EndsOn.HasValue && now > coupon.EndsOn.Value)
        {
            return new CouponResult { Valid = false, Code = coupon.Code, Message = $"{coupon.Code} has expired." };
        }

        if (subtotal < coupon.MinOrderValue)
        {
            return new CouponResult { Valid = false, Code = coupon.Code, Message = $"{coupon.Code} needs a cart value of ₹{coupon.MinOrderValue:N0} or more." };
        }

        decimal amount = 0;
        if (coupon.FixedAmount.HasValue)
        {
            amount = coupon.FixedAmount.Value;
        }
        else if (coupon.PercentDiscount.HasValue)
        {
            amount = Math.Round(subtotal * coupon.PercentDiscount.Value / 100m, 2);
            if (coupon.MaxDiscountAmount > 0 && amount > coupon.MaxDiscountAmount)
                amount = coupon.MaxDiscountAmount;
        }

        if (amount > subtotal)
            amount = subtotal;

        return new CouponResult
        {
            Valid = true,
            Code = coupon.Code,
            FreeShipping = coupon.FreeShipping,
            Amount = amount,
            Message = $"{coupon.Code} applied." + (coupon.FreeShipping ? " Free delivery included." : "")
        };
    }
}
