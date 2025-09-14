using System.ComponentModel.DataAnnotations;

namespace Kubrik.Models.Http;

public record LoginRequest
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}