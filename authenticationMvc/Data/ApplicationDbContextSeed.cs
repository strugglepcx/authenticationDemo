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
            if (!context.Users.Any())
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                var identityUser = new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "strugglepcx@126.com",
                    NormalizedUserName = "admin"
                };

                var result = await userManager.CreateAsync(identityUser, "pcx8421");

                if (!result.Succeeded)
                {
                    throw new Exception("初始化失败！");
                }
            }
        }
    }
}
