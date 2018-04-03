using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using authenticationMvc.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace authenticationMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).MigrationDbContext<ApplicationDbContext>((context, services) =>
            {
                new ApplicationDbContextSeed().SeedAsync(context, services).Wait();
            }).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000")
                .Build();
    }
}
