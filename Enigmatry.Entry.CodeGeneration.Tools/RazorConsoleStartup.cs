using System;
using System.IO;
using Enigmatry.Entry.TemplatingEngine;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Enigmatry.Entry.CodeGeneration.Tools;

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
                options.FileProviders.Add(new PhysicalFileProvider(Path.GetDirectoryName(AppContext.BaseDirectory)));
            });

        services.AddSingleton<RazorTemplatingEngine>();
    }

    [UsedImplicitly]
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }
}