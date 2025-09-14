using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Kubrik.Models.Devices;

using Microsoft.AspNetCore.Identity;

namespace Kubrik.Models.Identity;

public sealed class User : IdentityUser
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string Picture { get; set; } = "default.png";
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public Location Location { get; set; } = Location.Empty;
    
    [NotMapped]
    public List<Device> Devices { get; set; }
    
    [NotMapped]
    public List<RefreshToken> RefreshTokens { get; set; } = [];
}