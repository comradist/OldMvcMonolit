using Microsoft.AspNetCore.Identity;
using Northwind.Mvc.Models;
using Packt.Shared;

namespace Northwind.Mvc.Data;

public static class ContextSeed
{
    public static async Task SeedRolesAsync(UserManager<UserExtendedForIdentity> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any(r => r.Name == Enums.Roles.SuperAdmin.ToString()))
        {
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Basic.ToString()));
        }
    }

    public static async Task SeedSuperAdminAsync(UserManager<UserExtendedForIdentity> userManager, RoleManager<IdentityRole> roleManager)
    {
        var defaultAdmin = new UserExtendedForIdentity
        {
            UserName = "superadmin",
            Email = "superadmin@gmail.com",
            FirstName = "Poltos",
            LastName = "Poltosovich",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };

        if (!userManager.Users.Any(u => u.Id == defaultAdmin.Id))
        {
            if(CheckResult(await userManager.CreateAsync(defaultAdmin, "Pa$$w0rd")))
            {
                CheckResult(await userManager.AddToRoleAsync(defaultAdmin, Enums.Roles.Moderator.ToString()));
                await userManager.AddToRoleAsync(defaultAdmin, Enums.Roles.Moderator.ToString());
                await userManager.AddToRoleAsync(defaultAdmin, Enums.Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultAdmin, Enums.Roles.SuperAdmin.ToString());
                await userManager.UpdateAsync(defaultAdmin);
            }
        }
    }

    private static bool CheckResult(IdentityResult result)
    {
        if (result.Succeeded)
        {
            Console.WriteLine($"User Kek added to Basic successfully.");
            return true;
        }
        else
        {
            foreach (IdentityError error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
        return false;
    }
}
