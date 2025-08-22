using Microsoft.AspNetCore.Identity;
using SmartTaskPro.Models;

namespace SmartTaskPro.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleMgr = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userMgr = services.GetRequiredService<UserManager<User>>();

        var roles = new[] { "Admin", "Member" };
        foreach (var r in roles)
        {
            if (!await roleMgr.RoleExistsAsync(r))
                await roleMgr.CreateAsync(new IdentityRole<Guid>(r));
        }

        var adminEmail = "admin@smarttaskpro.local";
        var admin = await userMgr.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new User { Email = adminEmail, FullName = "Admin" };
            await userMgr.CreateAsync(admin, "Pass123!");
            await userMgr.AddToRoleAsync(admin, "Admin");
        }
    }
}
