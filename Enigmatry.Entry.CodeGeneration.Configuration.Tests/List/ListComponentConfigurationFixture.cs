using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;
using Enigmatry.Entry.CodeGeneration.Configuration.List;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests.List;

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
        componentModel.ComponentInfo.Feature.Name.Should().Be("Projects");
        componentModel.Columns.Count.Should().Be(5);
        componentModel.VisibleColumns.Count().Should().Be(4);
    }

    [Test]
    public void TestOrderByModel()
    {
        var configuration = new Configuration1();
        var builder = new ListComponentBuilder<Project>();

        builder.Component().OrderBy(OrderByType.Model);

        configuration.Configure(builder);
        var componentModel = builder.Build();

        componentModel.Should().NotBeNull();
        componentModel.Columns.Count.Should().Be(5);

        // Property order: Id, Title, StarDate, EndDate, Budget
        componentModel.ComponentInfo.OrderByType.Should().Be(OrderByType.Model);
        componentModel.Columns.ElementAt(0).Property.ToLower().Should().Be(nameof(Project.Id).ToLower());
        componentModel.Columns.ElementAt(1).Property.ToLower().Should().Be(nameof(Project.Title).ToLower());
        componentModel.Columns.ElementAt(2).Property.ToLower().Should().Be(nameof(Project.StartDate).ToLower());
        componentModel.Columns.ElementAt(3).Property.ToLower().Should().Be(nameof(Project.EndDate).ToLower());
        componentModel.Columns.ElementAt(4).Property.ToLower().Should().Be(nameof(Project.Budget).ToLower());
    }

    [Test]
    public void TestOrderByConfiguration()
    {
        var configuration = new Configuration1();
        var builder = new ListComponentBuilder<Project>();

        builder.Component().OrderBy(OrderByType.Configuration);

        configuration.Configure(builder);
        var componentModel = builder.Build();

        componentModel.Should().NotBeNull();
        componentModel.Columns.Count.Should().Be(5);

        // Configuration order: Id, Title, Budget, StartDate, EndDate
        componentModel.ComponentInfo.OrderByType.Should().Be(OrderByType.Configuration);
        componentModel.Columns.ElementAt(0).Property.ToLower().Should().Be(nameof(Project.Id).ToLower());
        componentModel.Columns.ElementAt(1).Property.ToLower().Should().Be(nameof(Project.Title).ToLower());
        componentModel.Columns.ElementAt(2).Property.ToLower().Should().Be(nameof(Project.Budget).ToLower());
        componentModel.Columns.ElementAt(3).Property.ToLower().Should().Be(nameof(Project.StartDate).ToLower());
        componentModel.Columns.ElementAt(4).Property.ToLower().Should().Be(nameof(Project.EndDate).ToLower());
    }

    [Test]
    public void TestIncludeUnconfiguredPropertiesIsTrueWhenNotConfigured()
    {
        var configuration = new Configuration1();
        var builder = new ListComponentBuilder<Project>();

        configuration.Configure(builder);
        var componentModel = builder.Build();

        // All properties are included as columns
        componentModel.ComponentInfo.IncludeUnconfiguredProperties.Should().BeTrue();
        componentModel.Columns.Count.Should().Be(5);
    }

    [Test]
    public void TestExcludeUnconfiguredProperties()
    {
        var configuration = new Configuration1();
        var builder = new ListComponentBuilder<Project>();

        builder.Component().IncludeUnconfiguredProperties(false);

        configuration.Configure(builder);
        var componentModel = builder.Build();
        
        // Only Id, Title and Budged are configured and created as columns
        componentModel.ComponentInfo.IncludeUnconfiguredProperties.Should().BeFalse();
        componentModel.Columns.Count.Should().Be(3);
        componentModel.Columns.ElementAt(0).Property.ToLower().Should().Be(nameof(Project.Id).ToLower());
        componentModel.Columns.ElementAt(1).Property.ToLower().Should().Be(nameof(Project.Title).ToLower());
        componentModel.Columns.ElementAt(2).Property.ToLower().Should().Be(nameof(Project.Budget).ToLower());
    }

    [Test]
    public void TestDefaultPropertyFormatterForDates()
    {
        var configuration = new Configuration1();
        var builder = new ListComponentBuilder<Project>();

        configuration.Configure(builder);
        var componentModel = builder.Build();

        componentModel.Columns.Where(x => x.Property == "startDate" || x.Property == "endDate")
            .All(x => x.Formatter.GetType() == typeof(DatePropertyFormatter))
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

            builder.Column(project => project.Title).WithHeaderName("Title");

            builder.Column(project => project.Budget).WithFormat(new CurrencyPropertyFormatter());
        }
    }
}
