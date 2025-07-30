using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Repository;

namespace App.API.Seeding
{
    public class ContextSeed
    {
        public static async Task ApplyRolesSeeding(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            if (!roleManager.Roles.Any())
            {
                string[] roleNames = { "student", "company", "mentor", "admin" };

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
        public static async Task ApplySocialMediaSeeding(MentorDbContext context, ILogger logger)
        {
            if (!context.SocialMedia.Any())
            {
                var socialMedia = new List<SocialMedia>()
                {
                    new SocialMedia
                    {
                        Title = "github",
                        BaseUrl = "https://github.com/",
                        SocialMediaLinks = new List<UserSocialMedia>
                        { new UserSocialMedia { AppUserId = "cd0714e9-b29e-4218-a050-d38fa63386f3", Username = "fawaz110" } }
                    },
                    new SocialMedia
                    {
                        Title = "instagram",
                        BaseUrl = "https://instagram.com/",
                        SocialMediaLinks = new List<UserSocialMedia>
                        { new UserSocialMedia { AppUserId = "cd0714e9-b29e-4218-a050-d38fa63386f3", Username = "1fa_waz" } }
                    },
                    new SocialMedia
                    {
                        Title = "facebook",
                        BaseUrl = "https://facebook.com/",
                        SocialMediaLinks = new List<UserSocialMedia>
                        { new UserSocialMedia { AppUserId = "cd0714e9-b29e-4218-a050-d38fa63386f3", Username = "mostafa.mohamedfawzi.5" } }
                    },
                    new SocialMedia
                    {
                        Title = "linkedin",
                        BaseUrl = "https://linkedin.com/in/",
                        SocialMediaLinks = new List<UserSocialMedia>
                        { new UserSocialMedia { AppUserId = "cd0714e9-b29e-4218-a050-d38fa63386f3", Username = "mustafa-mohamed-76b0b9239" } }
                    },
                    new SocialMedia
                    {
                        Title = "x",
                        BaseUrl = "https://x.com/",
                        SocialMediaLinks = new List<UserSocialMedia>
                        { new UserSocialMedia { AppUserId = "cd0714e9-b29e-4218-a050-d38fa63386f3", Username = "fawa_z_1" } }
                    },
                };

                await context.Set<SocialMedia>().AddRangeAsync(socialMedia);

                var result = await context.SaveChangesAsync();

                if (result <= 0)
                    logger.LogWarning("Nothing Added For SocialMedia");
                else
                    logger.LogWarning("SocialMedia Added Successfully");
            }
            else
                logger.LogWarning("No Need For SocialMedia Seeding");
        }
    }
}
