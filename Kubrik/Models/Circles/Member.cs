using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Kubrik.Models.Identity;

namespace Kubrik.Models.Circles;

public sealed class Member
{
    [Required]
    public string Id { get; set; }
    
    [Required]
    public string Nickname { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string CircleId { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [NotMapped]
    public Circle Circle { get; set; }
    
    [NotMapped]
    public User User { get; set; }
}