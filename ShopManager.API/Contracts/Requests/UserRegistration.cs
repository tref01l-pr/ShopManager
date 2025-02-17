using System.ComponentModel.DataAnnotations;
using ShopManager.Domain.Models;

namespace ShopManager.API.Contracts.Requests;

public class UserRegistration
{
    [Required]
    [EmailAddress]
    [MaxLength(User.MaxEmailLength)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
    
    [Required]
    [MaxLength(User.MaxNameLength)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(User.MaxNameLength)]
    public string LastName { get; set; }
    
    [Required]
    [MaxLength(User.MaxNameLength)]
    public string MiddleName { get; set; }
    
    [Required]
    public DateOnly DateOfBirth { get; set; }
}