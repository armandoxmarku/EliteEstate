#pragma warning disable CS8618
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteEstate.Models;
namespace EliteEstate.Models;

public class Agent
{
    [Key]
    public int AgentId { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    [UniqueEmail]
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string? ProfilePicture { get; set; }
    public string Bio { get; set; }
    
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; }       

    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Property>? Properties { get; set; } = new List<Property>();
    public List<Image>? Images { get; set; } = new List<Image>();
}