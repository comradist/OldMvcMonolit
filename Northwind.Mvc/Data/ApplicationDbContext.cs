﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Northwind.Mvc.Models;

namespace Northwind.Mvc.Data;

public class ApplicationDbContext : IdentityDbContext<UserExtendedForIdentity>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //! Need to be comented for update db

    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);
    //     //builder.HasDefaultSchema("Identity");

    //     builder.Entity<UserExtendedForIdentity>(entity =>
    //     {
    //         entity.ToTable(name: "User");
    //     });

    //     builder.Entity<IdentityRole>(entity =>
    //     {
    //         entity.ToTable(name: "Role");
    //     });

    //     builder.Entity<IdentityUserRole<string>>(entity =>
    //     {
    //         entity.ToTable(name: "UserRoles");
    //     });

    //     builder.Entity<IdentityUserClaim<string>>(entity =>
    //     {
    //         entity.ToTable(name: "UserClaims");
    //     });

    //     builder.Entity<IdentityUserLogin<string>>(entity =>
    //     {
    //         entity.ToTable(name: "UserLogins");
    //     });

    //     builder.Entity<IdentityRoleClaim<string>>(entity =>
    //     {
    //         entity.ToTable(name: "RoleClaims");
    //     });

    //     builder.Entity<IdentityUserToken<string>>(entity =>
    //     {
    //         entity.ToTable(name: "UserTokens");
    //     });
    // }
}
