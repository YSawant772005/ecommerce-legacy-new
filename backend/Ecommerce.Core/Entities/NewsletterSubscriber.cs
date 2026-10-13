namespace Ecommerce.Core.Entities;

public class NewsletterSubscriber
{
    public int Id { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    [System.ComponentModel.DataAnnotations.EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    public DateTime SubscribedOn { get; set; }
}
