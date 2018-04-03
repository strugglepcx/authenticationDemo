using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace authenticationMvc.Data
{
    public static class WebHostMigrationExtensions
    {
        public static IWebHost MigrationDbContext<TContext>(this IWebHost host,
            Action<TContext, IServiceProvider> action)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();

                try
                {
                    context.Database.Migrate();
                    action(context, services);

                    logger.LogInformation("Seed 成功！");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
            }
            return host;
        }
    }
}
