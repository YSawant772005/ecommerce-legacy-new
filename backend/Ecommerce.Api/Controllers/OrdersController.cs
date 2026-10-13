using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var userId = CurrentUserId;
        var user = await _context.Users.FindAsync(userId);

        var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        if (!cartItems.Any())
        {
            return BadRequest(new { error = "Cart is empty" });
        }

        var subtotal = cartItems.Sum(ci => ci.Product.Price * ci.Qty);
        var discount = 0m;
        var shipping = 49m;

        var userCart = await _context.UserCarts
            .FirstOrDefaultAsync(uc => uc.UserId == userId);

        if (!string.IsNullOrWhiteSpace(userCart?.CouponCode))
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == userCart.CouponCode && c.IsActive);

            if (coupon != null)
            {
                if (coupon.FixedAmount.HasValue)
                {
                    discount = coupon.FixedAmount.Value;
                }
                else if (coupon.PercentDiscount.HasValue)
                {
                    discount = decimal.Round(subtotal * coupon.PercentDiscount.Value / 100m, 2);
                    if (coupon.MaxDiscountAmount > 0 && discount > coupon.MaxDiscountAmount)
                        discount = coupon.MaxDiscountAmount;
                }

                if (coupon.FreeShipping)
                {
                    shipping = 0;
                }
            }
        }

        if (subtotal >= 999)
        {
            shipping = 0;
        }

        if (discount > subtotal)
            discount = subtotal;

        var tax = decimal.Round(subtotal * 0.18m, 2);
        var total = subtotal - discount + shipping + tax;

        var orderNumber = $"NK-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}";

        var order = new Order
        {
            OrderNumber = orderNumber,
            UserId = userId,
            Email = user?.Email ?? request.Email,
            FullName = user?.FullName ?? request.FullName,
            Phone = user?.Phone ?? request.Phone,
            AddressLine = request.AddressLine,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Subtotal = subtotal,
            Discount = discount,
            Shipping = shipping,
            Tax = tax,
            Total = total,
            Status = OrderStatus.Placed,
            PaymentMethod = request.PaymentMethod,
            PaymentRef = $"MOCK-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
            CouponCode = userCart?.CouponCode,
            CreatedOn = DateTime.UtcNow
        };

        foreach (var cartItem in cartItems)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                ImageUrl = cartItem.Product.ImageUrl,
                Slug = cartItem.Product.Slug,
                UnitPrice = cartItem.Product.Price,
                Quantity = cartItem.Qty
            });

            cartItem.Product.Stock -= cartItem.Qty;
        }

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);

        if (userCart != null)
        {
            userCart.CouponCode = null;
            userCart.UpdatedOn = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Email = order.Email,
            FullName = order.FullName,
            Phone = order.Phone,
            Subtotal = order.Subtotal,
            Discount = order.Discount,
            Shipping = order.Shipping,
            Tax = order.Tax,
            Total = order.Total,
            Status = order.Status.ToString(),
            PaymentMethod = order.PaymentMethod,
            CouponCode = order.CouponCode,
            CreatedOn = order.CreatedOn,
            Items = order.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductName = i.ProductName,
                ImageUrl = i.ImageUrl,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                LineTotal = i.LineTotal
            }).ToList()
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var userId = CurrentUserId;
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedOn)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Email = o.Email,
                FullName = o.FullName,
                Phone = o.Phone,
                Subtotal = o.Subtotal,
                Discount = o.Discount,
                Shipping = o.Shipping,
                Tax = o.Tax,
                Total = o.Total,
                Status = o.Status.ToString(),
                PaymentMethod = o.PaymentMethod,
                CouponCode = o.CouponCode,
                CreatedOn = o.CreatedOn,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductName = i.ProductName,
                    ImageUrl = i.ImageUrl,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    LineTotal = i.LineTotal
                }).ToList()
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var userId = CurrentUserId;
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Email = order.Email,
            FullName = order.FullName,
            Phone = order.Phone,
            Subtotal = order.Subtotal,
            Discount = order.Discount,
            Shipping = order.Shipping,
            Tax = order.Tax,
            Total = order.Total,
            Status = order.Status.ToString(),
            PaymentMethod = order.PaymentMethod,
            CouponCode = order.CouponCode,
            CreatedOn = order.CreatedOn,
            Items = order.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductName = i.ProductName,
                ImageUrl = i.ImageUrl,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                LineTotal = i.LineTotal
            }).ToList()
        });
    }
}
