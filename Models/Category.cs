#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteEstate.Models;
namespace EliteEstate.Models;
public class Category
{
    [Key]
    public int CategoryId { get; set; }
    
    [Required(ErrorMessage = "Category name is required")]
    [MinLength(3, ErrorMessage = "Category name must be at least 3 characters")]
    public string Name { get; set; }
    public List<Property>? Properties { get; set; } = new List<Property>();
}