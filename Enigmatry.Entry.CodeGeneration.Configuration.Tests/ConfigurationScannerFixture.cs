using Enigmatry.Entry.CodeGeneration.Configuration.List;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests;

[Category("unit")]
public class ConfigurationScannerFixture
{
    [Test]
    public void Test_ConfigurationScanner_ShouldGetComponentsFromComponentConfigurations()
    {
        var scanner = new ConfigurationScanner([typeof(Model1Configuration), typeof(Model2Configuration)]);

        var components = scanner.GetComponents().ToList();

        components.Count.ShouldBe(2);
        components[0].ComponentInfo.Name.ShouldBe("Model1");
        components[1].ComponentInfo.Name.ShouldBe("Model2");
    }

    [Test]
    public void Test_ConfigurationScanner_ShouldIgnoreTypesThatAreNotComponentConfigurations()
    {
        var scanner = new ConfigurationScanner([typeof(Model1Configuration), typeof(Model2)]);

        var components = scanner.GetComponents().ToList();

        components.Count.ShouldBe(1);
        components[0].ComponentInfo.Name.ShouldBe("Model1");
    }

    [Test]
    public void Test_ConfigurationScanner_ShouldIgnoreAbstractAndOrGenericComponentConfigurations()
    {
        var scanner = new ConfigurationScanner([typeof(Model2Configuration), typeof(GenericConfiguration<>), typeof(AbstractConfiguration)]);

        var components = scanner.GetComponents().ToList();

        components.Count.ShouldBe(1);
        components[0].ComponentInfo.Name.ShouldBe("Model2");
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
