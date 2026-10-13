namespace Ecommerce.Core.Entities;

public class OrderItem
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    
    public int? ProductId { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    public string ProductName { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.StringLength(300)]
    public string? ImageUrl { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(220)]
    public string? Slug { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal LineTotal => UnitPrice * Quantity;
    
    public virtual Order Order { get; set; } = null!;
    
    public virtual Product? Product { get; set; }
}
