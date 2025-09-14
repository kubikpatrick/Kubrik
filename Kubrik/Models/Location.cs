using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Kubrik.Models;

[Owned]
public sealed class Location
{
    [Required]
    public double Longitude { get; set; }
    
    [Required]
    public double Latitude { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; }

    [NotMapped]
    public LocationType Type { get; set; }
    
    [NotMapped]
    public static Location Empty => new Location
    {
        Longitude = 0,
        Latitude = 0
    };
}