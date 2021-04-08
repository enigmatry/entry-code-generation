using Enigmatry.Blueprint.BuildingBlocks.TemplatingEngine;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Enigmatry.Blueprint.CodeGeneration.Console
{
    public class RazorConsoleStartup
    {
        private readonly IHostEnvironment _environment;

        public RazorConsoleStartup(IConfiguration configuration, IHostEnvironment environment)
        {
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRazorPages()
                .AddRazorRuntimeCompilation(options =>
                {
                    options.FileProviders.Clear();
                    options.FileProviders.Add(new PhysicalFileProvider(Path.GetDirectoryName(AppContext.BaseDirectory) /*_environment.ContentRootPath*/));
                });

            services.AddSingleton<RazorTemplatingEngine>();

            services.AppSerilog();
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }
    }
}
