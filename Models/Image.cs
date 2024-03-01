#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteEstate.Models;
namespace EliteEstate.Models;

public class Image {
    [Key]
    public int ImageId { get; set; }
   
    public string ImagePath { get; set; }
    public int? PropertyId { get; set; }
    public Property? Property { get; set; }

    public int? AgentId { get; set; }
    public Agent? Agent { get; set; }
}