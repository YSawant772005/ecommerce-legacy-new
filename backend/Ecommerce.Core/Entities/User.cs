namespace Ecommerce.Core.Entities;

public class User
{
    public int Id { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(150)]
    public string FullName { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    [System.ComponentModel.DataAnnotations.EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(100)]
    public string PasswordSalt { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.StringLength(30)]
    public string? Role { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(30)]
    public string? Phone { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
    public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();
    
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    
    public virtual UserCart? UserCart { get; set; }
    
    public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
}
