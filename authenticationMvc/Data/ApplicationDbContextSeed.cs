using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authenticationMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace authenticationMvc.Data
{
    public class ApplicationDbContextSeed
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
        {
            if (!context.Roles.Any())
            {
                var roleManager = services.GetRequiredService<RoleManager<ApplicationUserRole>>();
                var role = new ApplicationUserRole
                {
                    Name = "Administrators",
                    NormalizedName = "Administrators"
                };

                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new Exception("初始化角色失败！");
                }
            }
            if (!context.Users.Any())
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                var identityUser = new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "strugglepcx@126.com",
                    NormalizedUserName = "admin",
                    Avatar = "http://www.biaobaiju.com/uploads/20180211/01/1518282836-IwHcULPdxW.jpg"
                };

                var result = await userManager.CreateAsync(identityUser, "pcx8421");

                if (!result.Succeeded)
                {
                    throw new Exception("初始化用户失败！");
                }

                await userManager.AddToRoleAsync(identityUser, "Administrators");
            }
        }
    }
}
