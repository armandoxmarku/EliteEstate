#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteEstate.Models;
namespace EliteEstate.Models;
public class Property
{
    [Key]
    public int PropertyId { get; set; }

    public string PropertyName { get; set; }

    public string Description { get; set; }
    
    public string Location { get; set; }
    public int Price { get; set; }
    public int Squaremetres { get; set; }
    public int Rooms { get; set; }
    public int? CategoryId { get; set; }
    public Category?  Category { get; set; }//
    public int? PropertyTypeId { get; set; }   
    public PropertyType? PropertyType { get; set; }
    public int? AgentId { get; set; } 
    public  Agent? Agent { get; set; }

    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public bool Garages { get; set; }
    public bool Pool { get; set; }
    public bool Garden { get; set; } 
    public List<Image>? Images { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}