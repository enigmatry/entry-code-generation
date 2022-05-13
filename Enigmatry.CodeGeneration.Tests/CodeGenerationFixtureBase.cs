using System;
using System.Reflection;
using Enigmatry.BuildingBlocks.TemplatingEngine;
using Enigmatry.CodeGeneration.Angular;
using Enigmatry.CodeGeneration.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Tests
{
    public abstract class CodeGenerationFixtureBase
    {
        private IHost _host = null!;
        private IServiceScope _testScope = null!;
        protected ITemplatingEngine _templatingEngine = null!;
        protected ITemplateWriter _templateWriter = null!;
        protected CodeGeneratorOptions _options = null!;
        protected IHtmlHelper _htmlHelper = null!;
        protected CodeGenerator _codeGenerator = null!;
        protected string _validatorsPath = "src/app/shared/custom-path";
        protected bool _enableI18N = false;

        [SetUp]
        public void Setup()
        {
            _options = new CodeGeneratorOptions(TestContext.CurrentContext.TestDirectory, Assembly.GetExecutingAssembly(), false)
            {
                Component = String.Empty,
                Framework = Framework.Angular,
                EnableI18N = _enableI18N,
                ValidatorsPath = _validatorsPath
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
                    services.AddSingleton<ITemplateWriterAppender, DisclaimerTemplateAppender>();
                    services.AddSingleton<CodeGenerator>();
                    services.AddSingleton(new AngularSettings(UiLibrary.Material));
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
