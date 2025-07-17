using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests.Form;

[Category("unit")]
public class FormComponentBuilderFixture
{
    [Test]
    public void TestBuildFormComponentModel()
    {
        var builder = new FormComponentBuilder<Project1>();

        var componentModel = builder.Build();

        componentModel.ShouldNotBeNull();
        componentModel.ComponentInfo.ShouldNotBeNull();
    }

    internal class Project1
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = String.Empty;
    }
}
