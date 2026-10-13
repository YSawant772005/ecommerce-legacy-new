namespace Ecommerce.Core.Services;

public interface IPricingService
{
    decimal CalculateTax(decimal subtotal);
    decimal CalculateShipping(decimal subtotal);
    decimal CalculateDiscount(decimal subtotal, string? couponCode);
    decimal CalculateTotal(decimal subtotal, decimal discount, decimal shipping, decimal tax);
}

public class CouponResult
{
    public bool Valid { get; set; }
    public bool FreeShipping { get; set; }
    public string? Code { get; set; }
    public decimal Amount { get; set; }
    public string Message { get; set; } = string.Empty;
}
