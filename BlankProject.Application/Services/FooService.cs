using BlankProject.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlankProject.Application.Services
{
    public class FooService : IFooService
    {
        public async Task RunAsync()
        {
            await Task.CompletedTask;
        }
    }
}
