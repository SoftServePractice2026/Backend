using Domain.Constants;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public static class IdentitySeed
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

            string[] roles = { Role.Admin, Role.User };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new ApplicationRole() { Name = role, NormalizedName = role.ToUpper() });
            }
        }
    }
}
