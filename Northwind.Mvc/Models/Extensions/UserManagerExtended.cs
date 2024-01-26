using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Northwind.Mvc.Models;

public static class UserManagerExtensions
{
    //! НЕ ПРАЦЮЄ ВИДАЛИ ЦЕ ВЗАГАЛІ

    public static Task SetFirstNameAsync(this UserManager<UserExtendedForIdentity> userManager, UserExtendedForIdentity user, string name)
    {
        if (user == null)
        {
            throw new ArgumentNullException("user");
        }
        
        return Task.FromResult(0);

    }

    public static Task<string> GetFirstNameAsync(this UserManager<UserExtendedForIdentity> userStore, UserExtendedForIdentity user)
    {
        if (user == null)
        {
            throw new ArgumentNullException("user");
        }
        return Task.FromResult(user.FirstName);

    }

    public static Task SetLastNameAsync(this UserManager<UserExtendedForIdentity> userStore, UserExtendedForIdentity user, string name)
    {
        if (user == null)
        {
            throw new ArgumentNullException("user");
        }
        user.LastName = name;
        return Task.FromResult(0);
    }

    public static Task<string> GetLastNameAsync(this UserManager<UserExtendedForIdentity> userStore, UserExtendedForIdentity user)
    {
        if (user == null)
        {
            throw new ArgumentNullException("user");
        }
        return Task.FromResult(user.LastName);

    }
}