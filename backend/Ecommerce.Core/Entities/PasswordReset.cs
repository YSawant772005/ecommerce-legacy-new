namespace Ecommerce.Core.Entities;

public class PasswordReset
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(200)]
    public string Token { get; set; } = string.Empty;
    
    public DateTime ExpiresOn { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public virtual User User { get; set; } = null!;
}

public class RefreshToken
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(500)]
    public string Token { get; set; } = string.Empty;
    
    public DateTime ExpiresOn { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public bool IsRevoked { get; set; }
    
    public virtual User User { get; set; } = null!;
}
