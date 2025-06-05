using Microsoft.AspNetCore.Identity;

namespace App.API.Seeding
{
    public class ContextSeed
    {
        public static async Task ApplyRolesSeeding(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            if (!roleManager.Roles.Any())
            {
                string[] roleNames = { "student", "mentor", "admin" };

                foreach (var roleName in roleNames)
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                        if (result.Succeeded)
                            logger.LogInformation("Role " + roleName + " added successfully <3");
                    }
            }
            else
                logger.LogWarning("No Need For Roles Seeding");
        }
    }
}
