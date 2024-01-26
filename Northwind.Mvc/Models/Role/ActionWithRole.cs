using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Northwind.Mvc.Models;

public class ValidateRole
{
    public string nameOfRole { get; set; }

    [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
    
    public string? nameUpdate { get; set; }
}


public class ActionWithRole
{
    public List<IdentityRole>? Roles { get; set; }

    public ValidateRole InputForValidateRole { get; set; }

}