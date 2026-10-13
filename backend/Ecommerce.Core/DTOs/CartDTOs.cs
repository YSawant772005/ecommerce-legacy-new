namespace Ecommerce.Core.DTOs;

public class CartItemDto
{
    public int ProductId { get; set; }
    public int Qty { get; set; }
    public string? Variant { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public class CartDto
{
    public List<CartItemDto> Items { get; set; } = new();
    public string? CouponCode { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Shipping { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
}

public class AddToCartRequest
{
    public int ProductId { get; set; }
    public int Qty { get; set; } = 1;
    public string? Variant { get; set; }
}

public class UpdateCartRequest
{
    public int ProductId { get; set; }
    public int Qty { get; set; }
}

public class ApplyCouponRequest
{
    public string CouponCode { get; set; } = string.Empty;
}
