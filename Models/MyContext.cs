#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace EliteEstate.Models;

public class MyContext : DbContext 
{     
    public MyContext(DbContextOptions options) : base(options) { }   
    public DbSet<User> Users { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    
}