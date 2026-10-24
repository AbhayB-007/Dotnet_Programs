using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TaskFlow.Infrastructure.Identity;

namespace TaskFlow.Infrastructure.Data;

// Runs once at startup: ensures DB is migrated + seeds the two roles and a
// default Admin account so you have something to log in with on first deploy.
public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        var context = provider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        foreach (var role in new[] { Roles.Admin, Roles.User })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        const string adminEmail = "admin@taskflow.local";
        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                EmailConfirmed = true,
                IsActive = true
            };
            // NOTE: change this password immediately after first deploy.
            var result = await userManager.CreateAsync(admin, "Admin@12345");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, Roles.Admin);
        }
    }
}
