namespace Ecommerce.Core.Entities;

public class Review
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(120)]
    public string AuthorName { get; set; } = string.Empty;
    
    public int Rating { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    public string? Title { get; set; }
    
    [System.ComponentModel.DataAnnotations.StringLength(2000)]
    public string? Body { get; set; }
    
    public bool VerifiedPurchase { get; set; }
    
    public bool IsApproved { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public virtual Product Product { get; set; } = null!;
}
