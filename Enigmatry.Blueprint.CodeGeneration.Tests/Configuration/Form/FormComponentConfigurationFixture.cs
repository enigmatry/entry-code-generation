using System;
using Enigmatry.Blueprint.CodeGeneration.Configuration.Form;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Blueprint.CodeGeneration.Tests.Configuration.Form
{
    [Category("unit")]
    public class FormComponentConfigurationFixture
    {
        [Test]
        public void TestFormComponentConfiguration()
        {
            var configuration = new Configuration();
            var builder = new FormComponentBuilder<ProjectDetails>();

            configuration.Configure(builder);
            var componentModel = builder.Build();

            componentModel.Should().NotBeNull();
            componentModel.ComponentInfo.Name.Should().Be("ProjectDetails");
            componentModel.ComponentInfo.FeatureName.Should().Be("Projects");

            componentModel.FormControls.Count.Should().Be(5);
        }

        internal class ProjectDetails
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = String.Empty;
            public string Description { get; set; } = String.Empty;
            public DateTimeOffset StartDate { get; set; }
            public DateTimeOffset EndDate { get; set; }
        }

        internal class CreateOrUpdateProjectCommand
        {
            public Guid? Id { get; set; }
            public string Title { get; set; } = "";
            public DateTimeOffset StartDate { get; set; }
            public DateTimeOffset EndDate { get; set; }
        }

        internal class Configuration : IFormComponentConfiguration<ProjectDetails>
        {
            public void Configure(FormComponentBuilder<ProjectDetails> builder)
            {
                builder.Component()
                    .HasName("ProjectDetails")
                    .BelongsToFeature("Projects");

                builder.HasCreateOrUpdateCommandOfType<CreateOrUpdateProjectCommand>();

                builder.FormControl(x => x.Id).IsVisible(false);

                builder.FormControl(x => x.Title)
                    .WithLabel("Title")
                    .WithPlaceholder("Title");

                builder.FormControl(x => x.StartDate)
                    .WithLabel("Start")
                    .WithPlaceholder("Start");
            }
        }
    }
}
