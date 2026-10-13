using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("")]
public class SitemapController : ControllerBase
{
    private readonly AppDbContext _context;

    public SitemapController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("sitemap.xml")]
    [Produces("application/xml")]
    public async Task<IActionResult> Sitemap()
    {
        var products = await _context.Products
            .Select(p => new
            {
                loc = $"https://novakart.com/products/{p.Slug}",
                lastmod = p.CreatedOn.ToString("yyyy-MM-dd"),
                priority = 0.8,
                changefreq = "weekly"
            })
            .ToListAsync();

        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .Select(c => new
            {
                loc = $"https://novakart.com/c/{c.Slug}",
                lastmod = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                priority = 0.9,
                changefreq = "daily"
            })
            .ToListAsync();

        var sitemap = new XDocument(
            new XElement("urlset",
                new XAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9"),
                categories.Select(c => new XElement("url",
                    new XElement("loc", c.loc),
                    new XElement("lastmod", c.lastmod),
                    new XElement("priority", c.priority),
                    new XElement("changefreq", c.changefreq)
                )),
                products.Select(p => new XElement("url",
                    new XElement("loc", p.loc),
                    new XElement("lastmod", p.lastmod),
                    new XElement("priority", p.priority),
                    new XElement("changefreq", p.changefreq)
                ))
            )
        );

        return Content(sitemap.ToString(), "application/xml");
    }
}
