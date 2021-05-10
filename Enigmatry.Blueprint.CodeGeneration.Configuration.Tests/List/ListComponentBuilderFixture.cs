using System;
using System.Linq;
using Enigmatry.Blueprint.CodeGeneration.Configuration.List;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Tests.List
{
    [Category("unit")]
    public class ListComponentBuilderFixture
    {
        [Test]
        public void TestBuildListComponentModel()
        {
            var builder = new ListComponentBuilder<Project1>();
            builder.Component().HasName("ProjectList");
            builder.Component().BelongsToFeature("Projects");

            var componentModel = builder.Build();

            componentModel.ComponentInfo.Name.Should().Be("ProjectList");
            componentModel.ComponentInfo.FeatureName.Should().Be("Projects");
        }

        [Test]
        public void TestColumnsAreVisibleByDefault()
        {
            var builder = new ListComponentBuilder<Project1>();
            var componentModel = builder.Build();
            componentModel.Columns.Count.Should().Be(2);
        }

        [Test]
        public void TestColumnsShouldBeConfigurable()
        {
            var builder = new ListComponentBuilder<Project1>();

            builder.Column(p => p.Title)
                .IsVisible(true)
                .IsSortable(false)
                .WithDisplayName("Test Title")
                .WithTranslationId("title");

            var componentModel = builder.Build();

            var column = componentModel.Columns.FirstOrDefault(c => c.DisplayName == "Test Title");
            column.Should().NotBeNull();
            column!.IsVisible.Should().BeTrue();
            column.IsSortable.Should().BeFalse();
            column.TranslationId.Should().Be("title");
        }

        internal class Project1
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = String.Empty;
        }
    }
}
