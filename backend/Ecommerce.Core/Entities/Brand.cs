namespace Ecommerce.Core.Entities;

public class Brand
{
    public int Id { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(120)]
    public string Name { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(140)]
    public string Slug { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.StringLength(6)]
    public string? Code { get; set; }
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
