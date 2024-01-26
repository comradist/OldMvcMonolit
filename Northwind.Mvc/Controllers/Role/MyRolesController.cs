using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Northwind.Mvc.Data;
using Northwind.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace Northwind.Mvc.Controllers;

public class MyRolesController : Controller
{
    private readonly ILogger<MyRolesController> logger;

    private readonly RoleManager<IdentityRole> roleManager;

    private readonly UserManager<UserExtendedForIdentity> userManager;

    public MyRolesController(RoleManager<IdentityRole> roleManager, UserManager<UserExtendedForIdentity> userManager, ILogger<MyRolesController> logger)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        this.logger = logger;
    }

    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> Index()
    {

        ActionWithRole actionWithRole = new()
        {
            Roles = await roleManager.Roles.ToListAsync()
        };
        return View(actionWithRole);
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> AddRoleAsync(ActionWithRole actionWithRole)
    {
        if(!ModelState.IsValid)
        {
            actionWithRole.Roles = await roleManager.Roles.ToListAsync();
            return View("Index", actionWithRole);
        }

        if (roleManager.Roles.Any(r => r.Name == actionWithRole.InputForValidateRole.nameUpdate))
        {
            return BadRequest("This role already exists");
        }

        await roleManager.CreateAsync(new IdentityRole(actionWithRole.InputForValidateRole.nameUpdate));
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> EditRoleAsync(ActionWithRole actionWithRole)
    {
        if (!ModelState.IsValid)
        {
            actionWithRole.Roles = await roleManager.Roles.ToListAsync();
            return View("Index", actionWithRole);
        }

        if (!roleManager.Roles.Any(r => r.Name == actionWithRole.InputForValidateRole.nameOfRole))
        {
            return BadRequest("This role not exists");
        }

        var role = await roleManager.FindByNameAsync(actionWithRole.InputForValidateRole.nameOfRole);
        if (role is not null)
        {
            role!.Name = actionWithRole.InputForValidateRole.nameUpdate;
            await roleManager.UpdateAsync(role);
        }

        return RedirectToAction("Index");
    }

    public async Task<ActionResult> DeleteRoleAsync(ActionWithRole actionWithRole)
    {
        if (!ModelState.IsValid)
        {
            actionWithRole.Roles = await roleManager.Roles.ToListAsync();
            return View("Index", actionWithRole);
        }

        if (!roleManager.Roles.Any(r => r.Name == actionWithRole.InputForValidateRole.nameOfRole))
        {
            return BadRequest("This role not exists");
        }

        var role = await roleManager.FindByNameAsync(actionWithRole.InputForValidateRole.nameOfRole);
        if (role is not null)
        {
            await roleManager.DeleteAsync(role);
        }
        return RedirectToAction("Index");

    }
    //http://localhost:5000/MyRoles/CreateSuperUser
    public async Task<ActionResult> CreateSuperUser()
    {
        logger.LogInformation("DB populate needed roles");
        //Create all needed roles and main admin, do it one time, when need to populate db
        await ContextSeed.SeedRolesAsync(userManager, roleManager);
        await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);

        return RedirectToAction("Index", "Home");
    }
}