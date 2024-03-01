#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteEstate.Models;

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
    	
        if(value == null)
        {
            return new ValidationResult("Email is required!");
        }
    
          MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));

    	if(_context.Users.Any(e => e.Email == value.ToString()) && _context.Agents.Any(e => e.Email != value.ToString()))
        {
            return new ValidationResult("Email must be unique!");
        } else {
            return ValidationResult.Success;
        }
    }
}