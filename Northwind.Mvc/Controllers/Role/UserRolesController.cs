using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Northwind.Mvc.Data;
using Northwind.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Mvc.Controllers;

public class UserRolesController : Controller
{
    private readonly RoleManager<IdentityRole> roleManager;

    private readonly UserManager<UserExtendedForIdentity> userManager;

    public UserRolesController(UserManager<UserExtendedForIdentity> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        List<UserInformation> listOfUserInformation = new();
        foreach (var user in userManager.Users)
        {
            UserInformation userInformation = new()
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                //RolesOfThisUser = new List<string>(await userManager.GetRolesAsync(user)),
            };
            userInformation.AllUserRoles = new();
            foreach(var role in roleManager.Roles)
            {
                UserRoles manageUserRoles = new()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    manageUserRoles.Selected = true;
                }
                else
                {
                    manageUserRoles.Selected = false;
                }

                userInformation.AllUserRoles.Add(manageUserRoles);
            }
            listOfUserInformation.Add(userInformation);
        }
        return View(listOfUserInformation);
    }

    public async Task<IActionResult> ManageRoleAsync(int userId)
    {
        // if(id == null)
        // {
        //     return BadRequest("Id is null");
        // }
        // if (!roleManager.Roles.Any(r => r.Id == id))
        // {
        //     return BadRequest("Invalid id");
        // }
        var selectedRoles = ViewData["SelectedRoles_" + userId] as List<string>;
        //var user = userManager.Users.Where(u => u.Id == userRolesViewModel.);

        return RedirectToAction("Index");
    }
}