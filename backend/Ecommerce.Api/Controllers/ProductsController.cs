using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Tagline = c.Tagline,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                DisplayOrder = c.DisplayOrder,
                IsActive = c.IsActive
            })
            .ToListAsync();

        return Ok(categories);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 24,
        [FromQuery] int? categoryId = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(term)) ||
                (p.Brand != null && p.Brand.Name.ToLower().Contains(term)) ||
                (p.Category != null && p.Category.Name.ToLower().Contains(term)));
        }

        query = sort switch
        {
            "price-asc" => query.OrderBy(p => p.Price),
            "price-desc" => query.OrderByDescending(p => p.Price),
            "rating" => query.OrderByDescending(p => p.Rating).ThenByDescending(p => p.RatingCount),
            "new" => query.OrderByDescending(p => p.CreatedOn),
            "discount" => query.OrderByDescending(p => p.ComparePrice == null ? 0 : p.ComparePrice - p.Price),
            _ => query.OrderByDescending(p => p.SoldCount)
        };

        var totalCount = await query.CountAsync();
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Sku = p.Sku,
                ShortDescription = p.ShortDescription,
                Description = p.Description,
                Price = p.Price,
                ComparePrice = p.ComparePrice,
                Stock = p.Stock,
                Rating = p.Rating,
                RatingCount = p.RatingCount,
                ReviewCount = p.ReviewCount,
                SoldCount = p.SoldCount,
                CategoryId = p.CategoryId,
                BrandId = p.BrandId,
                ImageUrl = p.ImageUrl,
                ImageUrl2 = p.ImageUrl2,
                ImageUrl3 = p.ImageUrl3,
                Colors = p.Colors,
                Sizes = p.Sizes,
                IsFeatured = p.IsFeatured,
                IsDeal = p.IsDeal,
                IsNew = p.IsNew,
                CreatedOn = p.CreatedOn,
                DiscountPercent = p.DiscountPercent,
                InStock = p.InStock,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name
            })
            .ToListAsync();

        return Ok(new PagedResult<ProductDto>
        {
            Items = products,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Sku = product.Sku,
            ShortDescription = product.ShortDescription,
            Description = product.Description,
            Price = product.Price,
            ComparePrice = product.ComparePrice,
            Stock = product.Stock,
            Rating = product.Rating,
            RatingCount = product.RatingCount,
            ReviewCount = product.ReviewCount,
            SoldCount = product.SoldCount,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId,
            ImageUrl = product.ImageUrl,
            ImageUrl2 = product.ImageUrl2,
            ImageUrl3 = product.ImageUrl3,
            Colors = product.Colors,
            Sizes = product.Sizes,
            IsFeatured = product.IsFeatured,
            IsDeal = product.IsDeal,
            IsNew = product.IsNew,
            CreatedOn = product.CreatedOn,
            DiscountPercent = product.DiscountPercent,
            InStock = product.InStock,
            CategoryName = product.Category.Name,
            BrandName = product.Brand.Name
        });
    }
}
