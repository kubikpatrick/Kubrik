using System.ComponentModel.DataAnnotations;

namespace Kubrik.Models.Http;

public record SignUpRequest
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
    
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Required]
    public string ConfirmPassword { get; set; }
}