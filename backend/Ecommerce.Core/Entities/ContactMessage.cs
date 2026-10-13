namespace Ecommerce.Core.Entities;

public class ContactMessage
{
    public int Id { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(150)]
    public string Name { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    [System.ComponentModel.DataAnnotations.EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    public string? Subject { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(2000)]
    public string Message { get; set; } = string.Empty;
    
    public DateTime CreatedOn { get; set; }
}
