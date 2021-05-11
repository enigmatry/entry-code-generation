using System;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.Formatters;
using Enigmatry.CodeGeneration.Configuration.List;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Tests.Configuration.List
{
    [Category("unit")]
    public class ListComponentConfigurationFixture
    {
        [Test]
        public void TestListComponentConfiguration()
        {
            var configuration = new Configuration1();
            var builder = new ListComponentBuilder<Project>();

            configuration.Configure(builder);
            var componentModel = builder.Build();

            componentModel.Should().NotBeNull();
            componentModel.ComponentInfo.Name.Should().Be("ProjectList");
            componentModel.ComponentInfo.FeatureName.Should().Be("Projects");
            componentModel.Columns.Count.Should().Be(5);
            componentModel.VisibleColumns.Count().Should().Be(4);

            componentModel.VisibleColumns
                .Where(x => x.Property == "startDate" || x.Property == "endDate")
                .All(x => x.Formatter.GetType() == typeof(DatePropertyFormatter))
                .Should().BeTrue();

            componentModel.VisibleColumns
                .Where(x => x.Property == "budget")
                .All(x => x.Formatter.GetType() == typeof(DecimalPropertyFormatter))
                .Should().BeTrue();
        }

        internal class Project
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = String.Empty;
            public DateTimeOffset StartDate { get; set; }
            public DateTimeOffset EndDate { get; set; }
            public decimal Budget { get; set; }
        }

        internal class Configuration1 : IListComponentConfiguration<Project>
        {
            public void Configure(ListComponentBuilder<Project> builder)
            {
                builder.Component()
                    .HasName("ProjectList")
                    .BelongsToFeature("Projects");

                builder.Column(project => project.Id).IsVisible(false);
            }
        }
    }
}
