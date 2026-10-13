namespace Ecommerce.Core.Entities;

public class Coupon
{
    public int Id { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(30)]
    public string Code { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(140)]
    public string Description { get; set; } = string.Empty;
    
    public decimal MinOrderValue { get; set; }
    
    public decimal? FixedAmount { get; set; }
    
    public decimal? PercentDiscount { get; set; }
    
    public decimal MaxDiscountAmount { get; set; }
    
    public bool FreeShipping { get; set; }
    
    public bool IsActive { get; set; }
    
    public int TotalUses { get; set; }
    
    public DateTime StartsOn { get; set; }
    
    public DateTime? EndsOn { get; set; }
    
    public DateTime CreatedOn { get; set; }
}
