using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReviewsController(AppDbContext context)
    {
        _context = context;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet("product/{productId}")]
    public async Task<ActionResult> GetProductReviews(int productId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.ProductId == productId && r.IsApproved)
            .OrderByDescending(r => r.CreatedOn)
            .Take(12)
            .Select(r => new
            {
                id = r.Id,
                authorName = r.AuthorName,
                rating = r.Rating,
                title = r.Title,
                body = r.Body,
                verifiedPurchase = r.VerifiedPurchase,
                createdOn = r.CreatedOn
            })
            .ToListAsync();

        return Ok(reviews);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> SubmitReview([FromBody] SubmitReviewRequest request)
    {
        var userId = CurrentUserId;
        var product = await _context.Products.FindAsync(request.ProductId);

        if (product == null)
        {
            return NotFound(new { error = "Product not found" });
        }

        var verified = await _context.OrderItems
            .AnyAsync(i => i.ProductId == request.ProductId && 
                          i.Order.UserId == userId && 
                          i.Order.Status == OrderStatus.Delivered);

        var review = new Review
        {
            ProductId = request.ProductId,
            AuthorName = (await _context.Users.FindAsync(userId))?.FullName ?? "Guest",
            Rating = request.Rating,
            Title = request.Title,
            Body = request.Body,
            VerifiedPurchase = verified,
            IsApproved = true,
            CreatedOn = DateTime.UtcNow
        };

        _context.Reviews.Add(review);

        // Update product rating
        var totalScore = product.Rating * product.RatingCount + request.Rating;
        product.RatingCount = product.RatingCount + 1;
        product.Rating = Math.Round(totalScore / product.RatingCount, 1);
        product.ReviewCount = product.ReviewCount + 1;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Review submitted successfully" });
    }
}

public class SubmitReviewRequest
{
    public int ProductId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
}
