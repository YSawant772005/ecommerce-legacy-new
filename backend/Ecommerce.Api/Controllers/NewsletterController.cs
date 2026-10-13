using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsletterController : ControllerBase
{
    private readonly AppDbContext _context;

    public NewsletterController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult> Subscribe([FromBody] SubscribeRequest request)
    {
        var existing = await _context.NewsletterSubscribers
            .FirstOrDefaultAsync(n => n.Email == request.Email);

        if (existing != null)
        {
            return BadRequest(new { error = "Email already subscribed" });
        }

        var subscriber = new NewsletterSubscriber
        {
            Email = request.Email,
            SubscribedOn = DateTime.UtcNow
        };

        _context.NewsletterSubscribers.Add(subscriber);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Subscribed successfully" });
    }
}

public class SubscribeRequest
{
    public string Email { get; set; } = string.Empty;
}
