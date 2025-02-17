using System.ComponentModel.DataAnnotations;

namespace ShopManager.API.Contracts.Requests;

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; }
    
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}