using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;

namespace BlankProject.Infra.Quartz
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseQuartz(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IJobFactory, JobFactory>();
                services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

                var jobConfig = hostContext.Configuration.GetSection("jobs").Get<JobConfig[]>();

                if (jobConfig == null)
                    throw new Exception("É necessário configurar os jobs do Quartz no arquivo de configuração (appSettings.environment.json).");

                var jobs = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(i => i.GetTypes())
                                    .Where(i => typeof(IJob).IsAssignableFrom(i) && !i.IsInterface && !i.IsInterface)
                                    .Join(jobConfig,
                                          i => i.Name, 
                                          i => i.TypeOf,
                                          (jClass, jConfig) => new { jClass, jConfig })
                                    .Where(i => !i.jConfig.Disabled)
                                    .Select(i => (i.jClass.Name, i.jClass.AssemblyQualifiedName, i.jConfig.Cron))
                                    .ToArray();

                foreach (var job in jobs)
                {
                var jobType = Type.GetType(job.AssemblyQualifiedName);

                services.AddSingleton(jobType);
                services.AddSingleton(i => new JobSchedule(name: job.Name, jobType: jobType, cronExpression: job.Cron));
                }

            services.AddHostedService<QuartzHostedService>();
            });

        return hostBuilder;
        }
    }
}
