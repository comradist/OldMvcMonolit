using Microsoft.AspNetCore.Identity;
namespace Northwind.Mvc.Models;

public class UserInformation
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<UserRoles> AllUserRoles { get; set; }
}

public class UserRoles
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public bool Selected { get; set; }
}
