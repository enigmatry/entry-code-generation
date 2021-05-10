using System.Linq;
using Enigmatry.Blueprint.CodeGeneration.Configuration;
using Enigmatry.Blueprint.CodeGeneration.Configuration.List;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Blueprint.CodeGeneration.Tests.Configuration
{
    [Category("unit")]
    public class ConfigurationScannerFixture
    {
        [Test]
        public void Test_ConfigurationScanner_ShouldGetComponentsFromComponentConfigurations()
        {
            var scanner = new ConfigurationScanner(new[] {typeof(Model1Configuration), typeof(Model2Configuration)});

            var components = scanner.GetComponents().ToList();

            components.Count.Should().Be(2);
            components[0].ComponentInfo.Name.Should().Be("Model1");
            components[1].ComponentInfo.Name.Should().Be("Model2");
        }

        [Test]
        public void Test_ConfigurationScanner_ShouldIgnoreTypesThatAreNotComponentConfigurations()
        {
            var scanner = new ConfigurationScanner(new[] {typeof(Model1Configuration), typeof(Model2) });

            var components = scanner.GetComponents().ToList();

            components.Count.Should().Be(1);
            components[0].ComponentInfo.Name.Should().Be("Model1");
        }

        internal class Model1
        {
        }

        internal class Model2
        {
        }

        internal class Model1Configuration : IListComponentConfiguration<Model1>
        {
            public void Configure(ListComponentBuilder<Model1> builder)
            {
            }
        }

        internal class Model2Configuration : IListComponentConfiguration<Model2>
        {
            public void Configure(ListComponentBuilder<Model2> builder)
            {
            }
        }
    }
}
