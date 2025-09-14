using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Kubrik.Models.Identity;

using Microsoft.EntityFrameworkCore;

namespace Kubrik.Models.Circles;

[PrimaryKey(nameof(Id))]
public sealed class Circle
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public string Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [NotMapped]
    public User User { get; set; }
    
    [NotMapped]
    public List<Member> Members { get; set; }
    
    [NotMapped]
    public List<User> Users => Members.Select(m => m.User).ToList();
}