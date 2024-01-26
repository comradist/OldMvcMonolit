using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Northwind.Mvc.Models;

public class UserExtendedForIdentity : IdentityUser
{
    
    public string FirstName { get; set; } 

    public string LastName { get; set;}

    public int? UsernameChangeLimit { get; set; } = 10;


    public byte[]? ProfilePicture { get; set; }
}