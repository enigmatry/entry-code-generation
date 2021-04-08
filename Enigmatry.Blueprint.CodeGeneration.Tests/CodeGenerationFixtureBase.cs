using System;
using System.Reflection;
using Enigmatry.Blueprint.BuildingBlocks.TemplatingEngine;
using Enigmatry.Blueprint.CodeGeneration.Angular;
using Enigmatry.Blueprint.CodeGeneration.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace Enigmatry.Blueprint.CodeGeneration.Tests
{
    public abstract class CodeGenerationFixtureBase
    {
        private IHost _host = null!;
        private IServiceScope _testScope = null!;
        protected ITemplatingEngine _templatingEngine = null!;
        protected ITemplateWriter _templateWriter = null!;
        protected CodeGenerationOptions _options = null!;
        protected IHtmlHelper _htmlHelper = null!;
        protected CodeGenerator _codeGenerator = null!;

        [SetUp]
        public void Setup()
        {
            _options = new CodeGenerationOptions(TestContext.CurrentContext.TestDirectory, Assembly.GetExecutingAssembly()) 
            {
                ComponentName = String.Empty, 
                Framework = Framework.Angular
            };

            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<RazorTestStartup>())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ITemplatingEngine, RazorTemplatingEngine>();
                    services.AddSingleton(_options);
                    services.AddSingleton<IComponentGenerator, AngularComponentGenerator>();
                    services.AddSingleton<IModuleGenerator, AngularModuleGenerator>();
                    services.AddSingleton<ITemplateRenderer, TemplateRenderer>();
                    services.AddSingleton<ITemplateWriter, InMemoryTemplateWriter>();
                    services.AddSingleton<CodeGenerator>();
                    services.AddSingleton(new AngularSettings());
                });

            _host = hostBuilder.Build();

            _testScope = CreateScope();

            _templatingEngine = GetService<RazorTemplatingEngine>();

            _templateWriter = GetService<ITemplateWriter>();

            _htmlHelper = GetService<IHtmlHelper>();

            _codeGenerator = GetService<CodeGenerator>();
        }

        private IServiceScope CreateScope()
        {
            var scopeFactory = _host.Services.GetRequiredService<IServiceScopeFactory>();
            return scopeFactory.CreateScope();
        }

        protected T GetService<T>() where T : notnull
        {
            return _testScope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
