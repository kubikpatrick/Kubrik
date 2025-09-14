using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Kubrik.Models.Identity;

using Microsoft.EntityFrameworkCore;

namespace Kubrik.Models.Devices;

[PrimaryKey(nameof(Id))]
public sealed class Device
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public string Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public bool IsPrincipal { get; set; }
    
    [Required]
    public DeviceType Type { get; set; }
    
    [Required]
    public Location Location { get; set; } = Location.Empty;
    
    [Required]
    public string UserId { get; set; }
    
    [NotMapped]
    public User User { get; set; }
}