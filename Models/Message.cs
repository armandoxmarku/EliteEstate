#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteEstate.Models;
namespace EliteEstate.Models;
public class Message
{
    [Key]
    public int MessageId { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public int UserId { get; set; } // Foreign Key
    public virtual User Sender { get; set; } // Navigation property
    public int AgentId { get; set; } // Foreign Key
    public virtual Agent Receiver { get; set; } // Navigation property
    public int PropertyId { get; set; } // Foreign Key
    public  Property Property { get; set; } // Navigation property
}
