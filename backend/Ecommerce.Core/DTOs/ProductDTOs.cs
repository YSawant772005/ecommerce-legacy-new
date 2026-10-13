namespace Ecommerce.Core.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Sku { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? ComparePrice { get; set; }
    public int Stock { get; set; }
    public double Rating { get; set; }
    public int RatingCount { get; set; }
    public int ReviewCount { get; set; }
    public int SoldCount { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageUrl2 { get; set; }
    public string? ImageUrl3 { get; set; }
    public string? Colors { get; set; }
    public string? Sizes { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsDeal { get; set; }
    public bool IsNew { get; set; }
    public DateTime CreatedOn { get; set; }
    public int DiscountPercent { get; set; }
    public bool InStock { get; set; }
    public string? CategoryName { get; set; }
    public string? BrandName { get; set; }
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Tagline { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class BrandDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Code { get; set; }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
