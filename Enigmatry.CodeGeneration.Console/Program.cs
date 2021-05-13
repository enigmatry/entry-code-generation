using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Enigmatry.Blueprint.BuildingBlocks.TemplatingEngine;
using Enigmatry.CodeGeneration.Angular;
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
        private static string _componentName = String.Empty;

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
                PrintBlankLines();
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
                new Option<string>(new[] { "--component-name", "-c" }, "Single component name to be generated")
            };
            rootCommand.Handler = CommandHandler.Create<string, string, string>(RootCommandHandler);
            return rootCommand;
        }

        private static async Task RootCommandHandler(
            string sourceAssembly,
            string destinationDirectory,
            string componentName)
        {
            _sourceAssembly = sourceAssembly;
            _destinationDirectory = destinationDirectory;
            _componentName = componentName;

            try
            {
                PrintIntro();

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
            var options = new CodeGeneratorOptions(_destinationDirectory, assembly)
            {
                ComponentName = _componentName
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


        private static void PrintIntro()
        {
            PrintBlankLines();
            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
            System.Console.WriteLine(@" █▀▀ █▄░█ █ █▀▀ █▀▄▀█ ▄▀█ ▀█▀ █▀█ █▄█");
            System.Console.WriteLine(@" ██▄ █░▀█ █ █▄█ █░▀░█ █▀█ ░█░ █▀▄ ░█░");
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(@"  ____  _                       _       _      _____          _         _____                           _             ");
            System.Console.WriteLine(@" |  _ \| |                     (_)     | |    / ____|        | |       / ____|                         | |            ");
            System.Console.WriteLine(@" | |_) | |_   _  ___ _ __  _ __ _ _ __ | |_  | |     ___   __| | ___  | |  __  ___ _ __   ___ _ __ __ _| |_ ___  _ __ ");
            System.Console.WriteLine(@" |  _ <| | | | |/ _ \ '_ \| '__| | '_ \| __| | |    / _ \ / _` |/ _ \ | | |_ |/ _ \ '_ \ / _ \ '__/ _` | __/ _ \| '__|");
            System.Console.WriteLine(@" | |_) | | |_| |  __/ |_) | |  | | | | | |_  | |___| (_) | (_| |  __/ | |__| |  __/ | | |  __/ | | (_| | || (_) | |   ");
            System.Console.WriteLine(@" |____/|_|\__,_|\___| .__/|_|  |_|_| |_|\__|  \_____\___/ \__,_|\___|  \_____|\___|_| |_|\___|_|  \__,_|\__\___/|_|   ");
            System.Console.WriteLine(@"                    | |                                                                                               ");
            System.Console.WriteLine(@"                    |_|                                                                                               ");
            System.Console.ForegroundColor = ConsoleColor.Gray;
            PrintBlankLines();
        }

        private static void PrintBlankLines(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                System.Console.WriteLine();
            }
        }
    }
}
