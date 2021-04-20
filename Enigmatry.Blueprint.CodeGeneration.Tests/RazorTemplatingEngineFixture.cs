using System.Collections.Generic;
using System.Threading.Tasks;
using Enigmatry.Blueprint.BuildingBlocks.TemplatingEngine;
using Enigmatry.Blueprint.CodeGeneration.Configuration;
using Enigmatry.Blueprint.CodeGeneration.Configuration.List;
using Enigmatry.Blueprint.CodeGeneration.Configuration.List.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace Enigmatry.Blueprint.CodeGeneration.Tests
{
    [Category("unit")]
    public class RazorTemplatingEngineFixture
    {
        private ITemplatingEngine _templatingEngine = null!;

        [SetUp]
        public void Setup()
        {
            IHost host = BuildHost();
            IServiceScopeFactory scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
            IServiceScope serviceScope = scopeFactory.CreateScope();
            _templatingEngine = serviceScope.ServiceProvider.GetRequiredService<RazorTemplatingEngine>();
        }

        [Test]
        public async Task TestRenderFromFile()
        {
            var componentModel = new ListComponentModel(new ComponentInfo(), new List<ColumnPropertyModel>());

            string contents = await _templatingEngine.RenderFromFileAsync("~/Templates/Angular/Material/Angular.List.Component.cshtml", componentModel);

            contents.Should().NotBeEmpty();
        }

        private static IHost BuildHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<RazorTestStartup>()).Build();
        }
    }
}
