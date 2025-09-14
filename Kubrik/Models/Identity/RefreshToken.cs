using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Kubrik.Models.Identity;

[PrimaryKey(nameof(Id))]
public sealed class RefreshToken
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public string Id { get; set; }
    
    [Required]
    public string Value { get; set; }
    
    [Required]
    public DateTime ExpiresAt { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [NotMapped]
    public User User { get; set; }
}