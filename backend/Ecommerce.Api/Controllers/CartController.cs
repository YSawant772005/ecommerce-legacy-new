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
public class CartController : ControllerBase
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart()
    {
        var userId = CurrentUserId;
        var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        var userCart = await _context.UserCarts
            .FirstOrDefaultAsync(uc => uc.UserId == userId);

        var couponCode = userCart?.CouponCode;

        var items = cartItems.Select(ci => new CartItemDto
        {
            ProductId = ci.ProductId,
            Qty = ci.Qty,
            Variant = ci.Variant,
            ProductName = ci.Product.Name,
            ImageUrl = ci.Product.ImageUrl,
            UnitPrice = ci.Product.Price,
            LineTotal = ci.Product.Price * ci.Qty
        }).ToList();

        var subtotal = items.Sum(i => i.LineTotal);
        var discount = 0m;
        var shipping = 49m;
        var tax = 0m;

        if (!string.IsNullOrWhiteSpace(couponCode))
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == couponCode.ToUpper() && c.IsActive);

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

        tax = decimal.Round(subtotal * 0.18m, 2);
        var total = subtotal - discount + shipping + tax;

        return Ok(new CartDto
        {
            Items = items,
            CouponCode = couponCode,
            Subtotal = subtotal,
            Discount = discount,
            Shipping = shipping,
            Tax = tax,
            Total = total
        });
    }

    [HttpPost("items")]
    public async Task<ActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var userId = CurrentUserId;
        var product = await _context.Products.FindAsync(request.ProductId);

        if (product == null)
        {
            return NotFound(new { error = "Product not found" });
        }

        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == request.ProductId);

        if (existingItem != null)
        {
            existingItem.Qty = Math.Min(10, existingItem.Qty + request.Qty);
            existingItem.UpdatedOn = DateTime.UtcNow;
        }
        else
        {
            var qty = Math.Min(10, Math.Max(1, request.Qty));
            _context.CartItems.Add(new CartItem
            {
                UserId = userId,
                ProductId = request.ProductId,
                Qty = qty,
                Variant = request.Variant,
                UpdatedOn = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Item added to cart" });
    }

    [HttpPut("items/{productId}")]
    public async Task<ActionResult> UpdateCartItem(int productId, [FromBody] UpdateCartRequest request)
    {
        var userId = CurrentUserId;
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

        if (cartItem == null)
        {
            return NotFound(new { error = "Item not found in cart" });
        }

        if (request.Qty <= 0)
        {
            _context.CartItems.Remove(cartItem);
        }
        else
        {
            cartItem.Qty = Math.Min(10, request.Qty);
            cartItem.UpdatedOn = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Cart updated" });
    }

    [HttpDelete("items/{productId}")]
    public async Task<ActionResult> RemoveCartItem(int productId)
    {
        var userId = CurrentUserId;
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

        if (cartItem == null)
        {
            return NotFound(new { error = "Item not found in cart" });
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Item removed from cart" });
    }

    [HttpDelete]
    public async Task<ActionResult> ClearCart()
    {
        var userId = CurrentUserId;
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);

        var userCart = await _context.UserCarts
            .FirstOrDefaultAsync(uc => uc.UserId == userId);
        if (userCart != null)
        {
            userCart.CouponCode = null;
            userCart.UpdatedOn = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Cart cleared" });
    }

    [HttpPost("coupon")]
    public async Task<ActionResult> ApplyCoupon([FromBody] ApplyCouponRequest request)
    {
        var userId = CurrentUserId;
        var coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.Code == request.CouponCode.ToUpper() && c.IsActive);

        if (coupon == null)
        {
            return BadRequest(new { error = "Invalid coupon code" });
        }

        var userCart = await _context.UserCarts
            .FirstOrDefaultAsync(uc => uc.UserId == userId);

        if (userCart == null)
        {
            userCart = new UserCart
            {
                UserId = userId,
                CouponCode = request.CouponCode.ToUpper(),
                UpdatedOn = DateTime.UtcNow
            };
            _context.UserCarts.Add(userCart);
        }
        else
        {
            userCart.CouponCode = request.CouponCode.ToUpper();
            userCart.UpdatedOn = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Coupon applied" });
    }
}
