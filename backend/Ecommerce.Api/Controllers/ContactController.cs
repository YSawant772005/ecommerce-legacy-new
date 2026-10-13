using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContactController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> SubmitMessage([FromBody] ContactMessage message)
    {
        message.CreatedOn = DateTime.UtcNow;
        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Message sent successfully" });
    }
}
