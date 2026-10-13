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
public class WishlistController : ControllerBase
{
    private readonly AppDbContext _context;

    public WishlistController(AppDbContext context)
    {
        _context = context;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet]
    public async Task<ActionResult> GetWishlist()
    {
        var userId = CurrentUserId;
        var wishlistItems = await _context.WishlistItems
            .Include(w => w.Product)
            .Where(w => w.UserId == userId)
            .ToListAsync();

        var items = wishlistItems.Select(w => new
        {
            productId = w.ProductId,
            productName = w.Product.Name,
            imageUrl = w.Product.ImageUrl,
            price = w.Product.Price,
            addedOn = w.AddedOn
        }).ToList();

        return Ok(items);
    }

    [HttpPost("{productId}")]
    public async Task<ActionResult> AddToWishlist(int productId)
    {
        var userId = CurrentUserId;
        var product = await _context.Products.FindAsync(productId);

        if (product == null)
        {
            return NotFound(new { error = "Product not found" });
        }

        var existing = await _context.WishlistItems
            .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

        if (existing != null)
        {
            _context.WishlistItems.Remove(existing);
        }
        else
        {
            _context.WishlistItems.Add(new WishlistItem
            {
                UserId = userId,
                ProductId = productId,
                AddedOn = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Wishlist updated" });
    }

    [HttpDelete("{productId}")]
    public async Task<ActionResult> RemoveFromWishlist(int productId)
    {
        var userId = CurrentUserId;
        var item = await _context.WishlistItems
            .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

        if (item == null)
        {
            return NotFound(new { error = "Item not in wishlist" });
        }

        _context.WishlistItems.Remove(item);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Removed from wishlist" });
    }
}
