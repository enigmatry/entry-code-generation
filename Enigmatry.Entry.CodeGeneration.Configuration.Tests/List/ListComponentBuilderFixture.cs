using Enigmatry.Entry.CodeGeneration.Configuration.List;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests.List;

[Category("unit")]
public class ListComponentBuilderFixture
{
    [Test]
    public void TestBuildListComponentModel()
    {
        var builder = new ListComponentBuilder<Project1>();
        builder.Component().HasName("ProjectList");
        builder.Component().BelongsToFeature("Projects");

        ListComponentModel componentModel = builder.Build();

        componentModel.ComponentInfo.Name.ShouldBe("ProjectList");
        componentModel.ComponentInfo.Feature.Name.ShouldBe("Projects");
    }

    [Test]
    public void TestColumnsAreVisibleByDefault()
    {
        var builder = new ListComponentBuilder<Project1>();
        ListComponentModel componentModel = builder.Build();
        componentModel.Columns.Count.ShouldBe(2);
    }

    [Test]
    public void TestColumnsShouldBeConfigurable()
    {
        var builder = new ListComponentBuilder<Project1>();

        builder.Column(p => p.Title)
            .IsVisible(true)
            .IsSortable(false)
            .WithHeaderName("Test Title")
            .WithTranslationId("title");

        ListComponentModel componentModel = builder.Build();

        Configuration.List.Model.ColumnDefinition? column = componentModel.Columns.FirstOrDefault(c => c.HeaderName == "Test Title");
        column.ShouldNotBeNull();
        column!.IsVisible.ShouldBeTrue();
        column.IsSortable.ShouldBeFalse();
        column.TranslationId.ShouldBe("title");
    }

    internal class Project1
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = String.Empty;
    }
}
