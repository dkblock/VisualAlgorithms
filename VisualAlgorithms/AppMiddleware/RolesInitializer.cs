using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.AppMiddleware
{
    public static class RolesInitializer
    {
        public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var applicationRoles = new List<string> { "admin", "user" };

            foreach (var role in applicationRoles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            //await InitializeAdminAccount(userManager);
        }

        private static async Task InitializeAdminAccount(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "";
            const string adminPassword = "";

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
