using System.ComponentModel.DataAnnotations;
using ShopManager.Domain.Models;

namespace ShopManager.API.Contracts.Requests;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(User.MaxEmailLength)]
    public string Email { get; set; }

    public string ReturnUrl { get; set; }
}