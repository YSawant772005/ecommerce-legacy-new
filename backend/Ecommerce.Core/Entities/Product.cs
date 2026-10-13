namespace Ecommerce.Core.Entities;

public class Product
{
    public int Id { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.StringLength(220)]
    public string? Slug { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(50)]
    public string? Sku { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(500)]
    public string? ShortDescription { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(4000)]
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
    
    [System.ComponentModel.DataAnnotations.StringLength(300)]
    public string? ImageUrl { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(300)]
    public string? ImageUrl2 { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(300)]
    public string? ImageUrl3 { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(240)]
    public string? Colors { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(160)]
    public string? Sizes { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    public string? Highlights { get; set; }
    
    public bool IsFeatured { get; set; }
    
    public bool IsDeal { get; set; }
    
    public bool IsNew { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public virtual Category Category { get; set; } = null!;
    
    public virtual Brand Brand { get; set; } = null!;
    
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    
    public int DiscountPercent
    {
        get
        {
            if (!ComparePrice.HasValue || ComparePrice.Value <= Price || ComparePrice.Value <= 0) return 0;
            return (int)Math.Round((ComparePrice.Value - Price) / ComparePrice.Value * 100);
        }
    }
    
    public bool InStock => Stock > 0;
    
    public string[] ColorList => string.IsNullOrWhiteSpace(Colors) ? Array.Empty<string>() : Colors.Split(',');
    
    public string[] SizeList => string.IsNullOrWhiteSpace(Sizes) ? Array.Empty<string>() : Sizes.Split(',');
}
