using BlankProject.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlankProject.TaskRunner.Jobs
{
    [DisallowConcurrentExecution]
    public class FooJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public FooJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var fooService = scope.ServiceProvider.GetRequiredService<IFooService>();

            await fooService.RunAsync();
        }
    }
}
