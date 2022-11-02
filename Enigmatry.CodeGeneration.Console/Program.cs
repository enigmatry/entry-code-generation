using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Enigmatry.Entry.TemplatingEngine;
using Enigmatry.CodeGeneration.Angular;
using Enigmatry.CodeGeneration.Console.Intro;
using Enigmatry.CodeGeneration.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Enigmatry.CodeGeneration.Console
{
    internal class Program
    {
        private static string _sourceAssembly = String.Empty;
        private static string _destinationDirectory = String.Empty;
        private static string _component = String.Empty;
        private static string _feature = String.Empty;
        private static string _validatorsPath = String.Empty;
        private static bool _enableI18n = false;

        private static async Task<int> Main(string[] args)
        {
            SerilogHelper.ConfigureSerilog();

            try
            {
                return await CreateRootCommand().InvokeAsync(args);
            }
            catch (Exception ex)
            {
                Log.Fatal("{@ex}", ex);
                return -1;
            }
            finally
            {
                System.Console.WriteLine();
                Log.CloseAndFlush();
            }
        }

        private static RootCommand CreateRootCommand()
        {
            var rootCommand = new RootCommand("Generates UI client feature component(s) from configuration(s)")
            {
                new Option<string>(new[] { "--source-assembly", "-sa" }, "Source assembly file from which component(s) configuration(s) will be read")
                {
                    IsRequired = true
                },
                new Option<string>(new[] { "--destination-directory", "-dd" }, "Destination directory where angular component(s) will be generated")
                {
                    IsRequired = true
                },
                new Option<string>(new[] { "--component", "-c" }, "Single component to be generated"),
                new Option<string>(new[] { "--feature", "-f" }, "Single feature to be generated"),
                new Option<string>(new[] { "--validators-path", "-vlp" }, "Destination of custom-validators.ts file"),
                new Option<bool>(new[] { "--enable-i18n", "-i" }, "Enable i18n")
            };
            rootCommand.Handler = CommandHandler.Create<string, string, string, string, string, bool>(RootCommandHandler);
            return rootCommand;
        }

        private static async Task RootCommandHandler(
            string sourceAssembly,
            string destinationDirectory,
            string component = "",
            string feature = "",
            string validatorsPath = "src/app/shared/validators/custom-validators",
            bool enableI18N = false)
        {
            _sourceAssembly = sourceAssembly;
            _destinationDirectory = destinationDirectory;
            _component = component;
            _feature = feature;
            _validatorsPath = validatorsPath;
            _enableI18n = enableI18N;

            try
            {
                new IntroGenerator().Print();

                var host = CreateHostBuilder().Build();

                using var scope = host.Services.CreateScope();
                var codeGenerator = scope.ServiceProvider.GetRequiredService<CodeGenerator>();

                await codeGenerator.Generate();
            }
            catch (Exception ex)
            {
                Log.Error($"Service host terminated unexpectedly!. Error: {ex}");
            }
        }

        private static IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder();
            return BuildHost<RazorConsoleStartup>(builder, containerBuilder => { });
        }

        private static IHostBuilder BuildHost<TStartup>(IHostBuilder builder, Action<ContainerBuilder> configureContainer) where TStartup : class
        {
            var assembly = Assembly.LoadFrom(_sourceAssembly);
            var options = new CodeGeneratorOptions(_destinationDirectory, assembly, _enableI18n)
            {
                Component = _component,
                Feature = _feature,
                ValidatorsPath = _validatorsPath
            };

            return builder
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ITemplatingEngine, RazorTemplatingEngine>();
                    services.AddSingleton(options);
                    services.AddSingleton<IComponentGenerator, AngularComponentGenerator>();
                    services.AddSingleton<IModuleGenerator, AngularModuleGenerator>();
                    services.AddSingleton<ITemplateRenderer, TemplateRenderer>();
                    services.AddSingleton<ITemplateWriter, TemplateWriter>();
                    services.AddSingleton<ITemplateWriterAppender, DisclaimerTemplateAppender>();
                    services.AddSingleton(new AngularSettings(UiLibrary.Material));
                    services.AddSingleton<CodeGenerator>();
                    services.AppSerilog();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<TStartup>(); })
                .ConfigureContainer(configureContainer)
                .UseConsoleLifetime();
        }
    }
}
