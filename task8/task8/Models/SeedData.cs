using Microsoft.AspNetCore.Identity;

public class SeedData
{
    public static async Task CreateRolesAndUsers(
        IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = serviceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

        // Create Roles
        string[] roles = { "User", "Admin", "SuperAdmin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create SuperAdmin
        string email = "superadmin@gmail.com";

        var superAdmin = await userManager.FindByEmailAsync(email);

        if (superAdmin == null)
        {
            superAdmin = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(superAdmin, "Super@12345");
            await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
        }
        string email2 = "admin@gmail.com";

        var Admin = await userManager.FindByEmailAsync(email2);

        if (Admin == null)
        {
            Admin = new IdentityUser
            {
                UserName = email2,
                Email = email2,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(Admin, "Admin@12345");
            await userManager.AddToRoleAsync(Admin, "Admin");
        }
    }
}