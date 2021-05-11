using System.Collections.Generic;
using System.Threading.Tasks;
using Enigmatry.Blueprint.BuildingBlocks.TemplatingEngine;
using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Configuration.List;
using Enigmatry.CodeGeneration.Configuration.List.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Tests
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
            var componentModel = new TestComponent();

            string contents = await _templatingEngine.RenderFromFileAsync("~/Templates/Angular/Material/Angular.List.Component.cshtml", componentModel);

            contents.Should().NotBeEmpty();
        }

        private static IHost BuildHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<RazorTestStartup>()).Build();
        }

        internal class TestComponent : ListComponentModel
        {
            public TestComponent() 
                : base(new ComponentInfo(), new RoutingInfo(""), new ApiClientInfo(""), new List<ColumnPropertyModel>())
            {
            }
        }
    }
}
