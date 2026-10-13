using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet("dashboard")]
    public async Task<ActionResult> GetDashboard()
    {
        var totalRevenue = await _context.Orders
            .Where(o => o.Status != OrderStatus.Cancelled)
            .SumAsync(o => o.Total);

        var monthlyRevenue = await _context.Orders
            .Where(o => o.Status != OrderStatus.Cancelled && o.CreatedOn >= DateTime.UtcNow.AddMonths(-1))
            .SumAsync(o => o.Total);

        var totalOrders = await _context.Orders.CountAsync();
        var totalCustomers = await _context.Users.CountAsync(u => u.Role == "Customer");
        var totalProducts = await _context.Products.CountAsync();

        var lowStock = await _context.Products
            .Where(p => p.Stock < 10)
            .CountAsync();

        var ordersByStatus = await _context.Orders
            .GroupBy(o => o.Status)
            .Select(g => new { status = g.Key.ToString(), count = g.Count() })
            .ToListAsync();

        var recentOrders = await _context.Orders
            .OrderByDescending(o => o.CreatedOn)
            .Take(10)
            .Select(o => new
            {
                id = o.Id,
                orderNumber = o.OrderNumber,
                email = o.Email,
                total = o.Total,
                status = o.Status.ToString(),
                createdOn = o.CreatedOn
            })
            .ToListAsync();

        return Ok(new
        {
            totalRevenue,
            monthlyRevenue,
            totalOrders,
            totalCustomers,
            totalProducts,
            lowStock,
            ordersByStatus,
            recentOrders
        });
    }

    [HttpGet("products")]
    public async Task<ActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _context.Products.Include(p => p.Category).Include(p => p.Brand).AsQueryable();
        var totalCount = await query.CountAsync();
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new { products, totalCount, page, pageSize });
    }

    [HttpPost("products")]
    public async Task<ActionResult> CreateProduct([FromBody] Product product)
    {
        product.CreatedOn = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return Ok(product);
    }

    [HttpPut("products/{id}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Name = product.Name;
        existing.Slug = product.Slug;
        existing.Price = product.Price;
        existing.ComparePrice = product.ComparePrice;
        existing.Stock = product.Stock;
        existing.CategoryId = product.CategoryId;
        existing.BrandId = product.BrandId;
        existing.ImageUrl = product.ImageUrl;
        existing.Description = product.Description;
        existing.IsFeatured = product.IsFeatured;
        existing.IsDeal = product.IsDeal;
        existing.IsNew = product.IsNew;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Product updated" });
    }

    [HttpDelete("products/{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Product deleted" });
    }

    [HttpGet("categories")]
    public async Task<ActionResult> GetCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        return Ok(categories);
    }

    [HttpPost("categories")]
    public async Task<ActionResult> CreateCategory([FromBody] Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Ok(category);
    }

    [HttpPut("categories/{id}")]
    public async Task<ActionResult> UpdateCategory(int id, [FromBody] Category category)
    {
        var existing = await _context.Categories.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Name = category.Name;
        existing.Slug = category.Slug;
        existing.Tagline = category.Tagline;
        existing.Description = category.Description;
        existing.ImageUrl = category.ImageUrl;
        existing.DisplayOrder = category.DisplayOrder;
        existing.IsActive = category.IsActive;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Category updated" });
    }

    [HttpDelete("categories/{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Category deleted" });
    }

    [HttpGet("brands")]
    public async Task<ActionResult> GetBrands()
    {
        var brands = await _context.Brands.ToListAsync();
        return Ok(brands);
    }

    [HttpPost("brands")]
    public async Task<ActionResult> CreateBrand([FromBody] Brand brand)
    {
        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();
        return Ok(brand);
    }

    [HttpPut("brands/{id}")]
    public async Task<ActionResult> UpdateBrand(int id, [FromBody] Brand brand)
    {
        var existing = await _context.Brands.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Name = brand.Name;
        existing.Slug = brand.Slug;
        existing.Code = brand.Code;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Brand updated" });
    }

    [HttpDelete("brands/{id}")]
    public async Task<ActionResult> DeleteBrand(int id)
    {
        var brand = await _context.Brands.FindAsync(id);
        if (brand == null) return NotFound();

        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Brand deleted" });
    }

    [HttpGet("orders")]
    public async Task<ActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null)
    {
        var query = _context.Orders.Include(o => o.Items).AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            {
                query = query.Where(o => o.Status == orderStatus);
            }
        }

        var totalCount = await query.CountAsync();
        var orders = await query
            .OrderByDescending(o => o.CreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new { orders, totalCount, page, pageSize });
    }

    [HttpGet("orders/{id}")]
    public async Task<ActionResult> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPut("orders/{id}/status")]
    public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        var oldStatus = order.Status;
        order.Status = request.Status;

        // Restock if cancelled
        if (request.Status == OrderStatus.Cancelled && oldStatus != OrderStatus.Cancelled)
        {
            foreach (var item in order.Items)
            {
                if (item.Product != null)
                {
                    item.Product.Stock += item.Quantity;
                }
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Order status updated" });
    }

    [HttpGet("customers")]
    public async Task<ActionResult> GetCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var customers = await _context.Users
            .Where(u => u.Role == "Customer")
            .Include(u => u.Orders)
            .Select(u => new
            {
                id = u.Id,
                fullName = u.FullName,
                email = u.Email,
                phone = u.Phone,
                orderCount = u.Orders.Count,
                totalSpend = u.Orders.Sum(o => o.Total)
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await _context.Users.CountAsync(u => u.Role == "Customer");
        return Ok(new { customers, totalCount, page, pageSize });
    }

    [HttpGet("coupons")]
    public async Task<ActionResult> GetCoupons()
    {
        var coupons = await _context.Coupons.ToListAsync();
        return Ok(coupons);
    }

    [HttpPost("coupons")]
    public async Task<ActionResult> CreateCoupon([FromBody] Coupon coupon)
    {
        coupon.CreatedOn = DateTime.UtcNow;
        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();
        return Ok(coupon);
    }

    [HttpPut("coupons/{id}")]
    public async Task<ActionResult> UpdateCoupon(int id, [FromBody] Coupon coupon)
    {
        var existing = await _context.Coupons.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Code = coupon.Code;
        existing.Description = coupon.Description;
        existing.MinOrderValue = coupon.MinOrderValue;
        existing.FixedAmount = coupon.FixedAmount;
        existing.PercentDiscount = coupon.PercentDiscount;
        existing.MaxDiscountAmount = coupon.MaxDiscountAmount;
        existing.FreeShipping = coupon.FreeShipping;
        existing.IsActive = coupon.IsActive;
        existing.StartsOn = coupon.StartsOn;
        existing.EndsOn = coupon.EndsOn;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Coupon updated" });
    }

    [HttpDelete("coupons/{id}")]
    public async Task<ActionResult> DeleteCoupon(int id)
    {
        var coupon = await _context.Coupons.FindAsync(id);
        if (coupon == null) return NotFound();

        _context.Coupons.Remove(coupon);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Coupon deleted" });
    }

    [HttpGet("messages")]
    public async Task<ActionResult> GetMessages()
    {
        var messages = await _context.ContactMessages
            .OrderByDescending(m => m.CreatedOn)
            .ToListAsync();
        return Ok(messages);
    }

    [HttpDelete("messages/{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message == null) return NotFound();

        _context.ContactMessages.Remove(message);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Message deleted" });
    }

    [HttpGet("reviews")]
    public async Task<ActionResult> GetReviews([FromQuery] int? productId = null)
    {
        var query = _context.Reviews.Include(r => r.Product).AsQueryable();
        
        if (productId.HasValue)
        {
            query = query.Where(r => r.ProductId == productId.Value);
        }

        var reviews = await query
            .OrderByDescending(r => r.CreatedOn)
            .ToListAsync();
        return Ok(reviews);
    }

    [HttpPut("reviews/{id}/approve")]
    public async Task<ActionResult> ApproveReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null) return NotFound();

        review.IsApproved = true;
        await _context.SaveChangesAsync();
        return Ok(new { message = "Review approved" });
    }

    [HttpPut("reviews/{id}/hide")]
    public async Task<ActionResult> HideReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null) return NotFound();

        review.IsApproved = false;
        await _context.SaveChangesAsync();
        return Ok(new { message = "Review hidden" });
    }

    [HttpDelete("reviews/{id}")]
    public async Task<ActionResult> DeleteReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null) return NotFound();

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Review deleted" });
    }

    [HttpGet("newsletter")]
    public async Task<ActionResult> GetSubscribers()
    {
        var subscribers = await _context.NewsletterSubscribers
            .OrderByDescending(n => n.SubscribedOn)
            .ToListAsync();
        return Ok(subscribers);
    }

    [HttpDelete("newsletter/{id}")]
    public async Task<ActionResult> DeleteSubscriber(int id)
    {
        var subscriber = await _context.NewsletterSubscribers.FindAsync(id);
        if (subscriber == null) return NotFound();

        _context.NewsletterSubscribers.Remove(subscriber);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Subscriber deleted" });
    }
}

public class UpdateStatusRequest
{
    public OrderStatus Status { get; set; }
}
