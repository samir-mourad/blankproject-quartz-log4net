using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace BlankProject.Infra.Log4Net
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseLog4Net(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureLogging((hostContext, logger) =>
            {
                var config = hostContext.Configuration.GetSection("log4net").Get<Log4NetConfig>();
                var path = config.Path ?? Directory.GetCurrentDirectory();
                var filename = config.Filename ?? $"{DateTime.Today}.log";
                var configFilename = config.ConfigFilename ?? "log4net.config";

                var options = new Log4NetProviderOptions();
                options.Watch = true;
                options.Log4NetConfigFileName = configFilename;
                log4net.GlobalContext.Properties["logFileName"] = Path.Combine(path, filename);

                logger.AddConsole();
                logger.AddLog4Net();

                if (config?.Filters?.Any() ?? false)
                {
                    foreach (var filter in config.Filters)
                    {
                        logger.AddFilter(filter.TypeOf, filter.LogLevel);
                    }
                }
            });

            return hostBuilder;
        }
    }

    class Log4NetConfig
    {
        public string Path { get; set; }
        public string Filename { get; set; }
        public string ConfigFilename { get; set; }
        public FilterConfig[] Filters { get; set; }

    }

    class FilterConfig
    {
        public string TypeOf { get; set; }

        public LogLevel LogLevel { get; set; }
    }
}
