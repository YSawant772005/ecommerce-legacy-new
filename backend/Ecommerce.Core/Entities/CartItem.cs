namespace Ecommerce.Core.Entities;

public class CartItem
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int ProductId { get; set; }
    
    public int Qty { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(120)]
    public string? Variant { get; set; }
    
    public DateTime UpdatedOn { get; set; }
    
    public virtual User User { get; set; } = null!;
    
    public virtual Product Product { get; set; } = null!;
}

public class UserCart
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(30)]
    public string? CouponCode { get; set; }
    
    public DateTime UpdatedOn { get; set; }
    
    public virtual User User { get; set; } = null!;
}

public class WishlistItem
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int ProductId { get; set; }
    
    public DateTime AddedOn { get; set; }
    
    public virtual User User { get; set; } = null!;
    
    public virtual Product Product { get; set; } = null!;
}
