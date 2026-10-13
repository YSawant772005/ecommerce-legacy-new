namespace Ecommerce.Core.Entities;

public enum OrderStatus
{
    Placed = 0,
    Packed = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4
}

public class Order
{
    public int Id { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(40)]
    public string OrderNumber { get; set; } = string.Empty;
    
    public int? UserId { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    public string Email { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(150)]
    public string FullName { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.StringLength(30)]
    public string? Phone { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(400)]
    public string AddressLine { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(120)]
    public string City { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(120)]
    public string State { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(20)]
    public string PostalCode { get; set; } = string.Empty;
    
    public decimal Subtotal { get; set; }
    
    public decimal Discount { get; set; }
    
    public decimal Shipping { get; set; }
    
    public decimal Tax { get; set; }
    
    public decimal Total { get; set; }
    
    public OrderStatus Status { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(30)]
    public string? PaymentMethod { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(60)]
    public string? PaymentRef { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(30)]
    public string? CouponCode { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public virtual User? User { get; set; }
    
    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
