using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Serilog;

namespace MyCardNuke
{
    public class Program
    {
        public static void Main(string[] args)

        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   
                   .ConfigureAppConfiguration((context, cbuilder) => cbuilder.SetBasePath(context.HostingEnvironment.ContentRootPath)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                   .AddEnvironmentVariables())
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseIISIntegration()
                   .UseKestrel()
                   .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                               .ReadFrom.Configuration(hostingContext.Configuration)
                               .Enrich.FromLogContext()
                               .WriteTo.Console()
                               )
                .UseStartup<Startup>()

                   ;
    }
}
