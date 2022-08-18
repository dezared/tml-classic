using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KeklandBankSystem.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KeklandBankSystem
{
    public class Program
    {
        public static string systemCofnig = Environment.GetEnvironmentVariable("SystemConfiguration");
        public static string SystemConfiguration { get; set; }

        public static void Main(string[] args)
        {
            if(systemCofnig == "TestingSystem") SystemConfiguration = "TestDbKekbk";
            else if(systemCofnig == "PublishSystem") SystemConfiguration = "DbKekbk";
            else SystemConfiguration = "Develop";

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging((context, logger) =>
                    {
                        logger.AddConsole();
                        logger.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error);
                        logger.AddFilter("Microsoft.EntityFrameworkCore.Query", LogLevel.Error);

                    });
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Any, Convert.ToInt32(Environment.GetEnvironmentVariable("PORT")));
                    }).UseStartup<Startup>();
                    //webBuilder.UseUrls(Environment.GetEnvironmentVariable($"KestrelIps_{SystemConfiguration}"));
                }).Build().Run();
        }
    }
}
