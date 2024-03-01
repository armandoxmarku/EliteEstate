#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteEstate.Models;
namespace EliteEstate.Models;


public class Favorite
{
    [Key]
    public int FavoriteId { get; set; }
    public int UserId { get; set; } // Foreign Key
    public User User { get; set; } // Navigation property
    public int PropertyId { get; set; } // Foreign Key
    public Property Property { get; set; } // Navigation property
}
