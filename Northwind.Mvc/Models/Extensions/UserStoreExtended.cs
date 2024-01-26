using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Northwind.Mvc.Models;

public static class UserStoreExtensions
{
    //! НЕ ПРАЦЮЄ ВИДАЛИ ЦЕ ВЗАГАЛІ
    
    public static Task SetFirstNameAsync(this UserStore<UserExtendedForIdentity> userStore, UserExtendedForIdentity user, string name)
    {
        if (user == null)
        {
            throw new ArgumentNullException("user");
        }
        user.FirstName = name;
        return Task.FromResult(0);

    }

    public static Task<string> GetFirstNameAsync(this UserStore<UserExtendedForIdentity> userStore, UserExtendedForIdentity user)
    {
        if (user == null)
        {
            throw new ArgumentNullException("user");
        }
        return Task.FromResult(user.FirstName);

    }

    public static Task SetLastNameAsync(this UserStore<UserExtendedForIdentity> userStore, UserExtendedForIdentity user, string name)
    {
        if (user == null)
        {
            throw new ArgumentNullException("user");
        }
        user.LastName = name;
        return Task.FromResult(0);
    }

    public static Task<string> GetLastNameAsync(this UserStore<UserExtendedForIdentity> userStore, UserExtendedForIdentity user)
    {
        if (user == null)
        {
            throw new ArgumentNullException("user");
        }
        return Task.FromResult(user.LastName);

    }
}