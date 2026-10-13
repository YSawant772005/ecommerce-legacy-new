using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Entities;

public class CustomerAddress
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(30)]
    [RegularExpression(@"[0-9+\-\s]{8,15}", ErrorMessage = "Enter a valid phone number")]
    public string Phone { get; set; } = string.Empty;
    
    [Required]
    [StringLength(400)]
    public string AddressLine { get; set; } = string.Empty;
    
    [Required]
    [StringLength(120)]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [StringLength(120)]
    public string State { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    [RegularExpression(@"[0-9]{6}", ErrorMessage = "PIN code must be 6 digits")]
    public string PostalCode { get; set; } = string.Empty;
    
    [Required]
    [StringLength(80)]
    public string Country { get; set; } = string.Empty;
    
    public bool IsDefault { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public virtual User User { get; set; } = null!;
}
