using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet("profile")]
    public async Task<ActionResult> GetProfile()
    {
        var userId = CurrentUserId;
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = user.Id,
            fullName = user.FullName,
            email = user.Email,
            phone = user.Phone,
            role = user.Role
        });
    }

    [HttpPut("profile")]
    public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = CurrentUserId;
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        user.FullName = request.FullName;
        user.Phone = request.Phone;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Profile updated" });
    }

    [HttpGet("addresses")]
    public async Task<ActionResult> GetAddresses()
    {
        var userId = CurrentUserId;
        var addresses = await _context.CustomerAddresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ToListAsync();

        return Ok(addresses);
    }

    [HttpPost("addresses")]
    public async Task<ActionResult> AddAddress([FromBody] CustomerAddress address)
    {
        var userId = CurrentUserId;
        address.UserId = userId;
        address.CreatedOn = DateTime.UtcNow;

        _context.CustomerAddresses.Add(address);
        await _context.SaveChangesAsync();

        return Ok(address);
    }

    [HttpPut("addresses/{id}")]
    public async Task<ActionResult> UpdateAddress(int id, [FromBody] CustomerAddress address)
    {
        var userId = CurrentUserId;
        var existing = await _context.CustomerAddresses
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (existing == null)
        {
            return NotFound();
        }

        existing.FullName = address.FullName;
        existing.Phone = address.Phone;
        existing.AddressLine = address.AddressLine;
        existing.City = address.City;
        existing.State = address.State;
        existing.PostalCode = address.PostalCode;
        existing.Country = address.Country;
        existing.IsDefault = address.IsDefault;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Address updated" });
    }

    [HttpDelete("addresses/{id}")]
    public async Task<ActionResult> DeleteAddress(int id)
    {
        var userId = CurrentUserId;
        var address = await _context.CustomerAddresses
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (address == null)
        {
            return NotFound();
        }

        _context.CustomerAddresses.Remove(address);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Address deleted" });
    }

    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = CurrentUserId;
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var passwordHasher = new PasswordHasher();
        if (!passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest(new { error = "Current password is incorrect" });
        }

        var newSalt = passwordHasher.GenerateSalt();
        user.PasswordHash = passwordHasher.HashPassword(request.NewPassword, newSalt);
        user.PasswordSalt = newSalt;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Password changed successfully" });
    }
}

public class UpdateProfileRequest
{
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
