using Enigmatry.Entry.CodeGeneration.Configuration.List;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests;

[Category("unit")]
public class ConfigurationScannerFixture
{
    [Test]
    public void Test_ConfigurationScanner_ShouldGetComponentsFromComponentConfigurations()
    {
        var scanner = new ConfigurationScanner(new[] { typeof(Model1Configuration), typeof(Model2Configuration) });

        List<IComponentModel> components = scanner.GetComponents().ToList();

        components.Count.Should().Be(2);
        components[0].ComponentInfo.Name.Should().Be("Model1");
        components[1].ComponentInfo.Name.Should().Be("Model2");
    }

    [Test]
    public void Test_ConfigurationScanner_ShouldIgnoreTypesThatAreNotComponentConfigurations()
    {
        var scanner = new ConfigurationScanner(new[] { typeof(Model1Configuration), typeof(Model2) });

        List<IComponentModel> components = scanner.GetComponents().ToList();

        components.Count.Should().Be(1);
        components[0].ComponentInfo.Name.Should().Be("Model1");
    }

    [Test]
    public void Test_ConfigurationScanner_ShouldIgnoreAbstractAndOrGenericComponentConfigurations()
    {
        var scanner = new ConfigurationScanner(new[] { typeof(Model2Configuration), typeof(GenericConfiguration<>), typeof(AbstractConfiguration) });

        List<IComponentModel> components = scanner.GetComponents().ToList();

        components.Count.Should().Be(1);
        components[0].ComponentInfo.Name.Should().Be("Model2");
    }

    internal class Model1
    {
    }

    internal class Model2
    {
    }

    internal class Model3
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

    internal class GenericConfiguration<T> : IListComponentConfiguration<T> where T : class
    {
        public void Configure(ListComponentBuilder<T> builder)
        {
        }
    }

    internal abstract class AbstractConfiguration : IListComponentConfiguration<Model3>
    {
        public void Configure(ListComponentBuilder<Model3> builder)
        {
        }
    }
}
