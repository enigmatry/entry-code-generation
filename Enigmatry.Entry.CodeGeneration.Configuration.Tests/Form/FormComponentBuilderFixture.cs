using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests.Form;

[Category("unit")]
public class FormComponentBuilderFixture
{
    [Test]
    public void TestBuildFormComponentModel()
    {
        var builder = new FormComponentBuilder<Project1>();

        var componentModel = builder.Build();

        componentModel.Should().NotBeNull();
        componentModel.ComponentInfo.Should().NotBeNull();
    }

    internal class Project1
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = String.Empty;
    }
}
