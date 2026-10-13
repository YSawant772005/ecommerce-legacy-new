namespace Ecommerce.Core.Entities;

public class Category
{
    public int Id { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(120)]
    public string Name { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(140)]
    public string Slug { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.StringLength(240)]
    public string? Tagline { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(1000)]
    public string? Description { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(300)]
    public string? ImageUrl { get; set; }
    
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
