using BlankProject.Infra.IoC;
using BlankProject.Infra.Log4Net;
using BlankProject.Infra.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace BlankProject.TaskRunner
{
    class Program
    {
        private static string _environment;

        static void Main(string[] args)
        {
            ConfigureEnvironment();
            CreateHostBuilder(args).Build().Run();            
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureAppConfiguration((host, config) =>
                       {
                           config.SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appSettings.json", optional: false)
                                 .AddJsonFile($"appSettings.{_environment}.json", optional: false);
                       })
                       .ConfigureServices((hostingContext, services) =>
                       {
                           DependencyInjectionConfig.Register(services, hostingContext.Configuration);
                       })
                       .UseLog4Net()
                       .UseQuartz()
                       .UseWindowsService();
        }

        public static void ConfigureEnvironment()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());

            var config = builder.AddJsonFile("appSettings.json").Build();

            _environment = config["environment"];
        }
    }
}