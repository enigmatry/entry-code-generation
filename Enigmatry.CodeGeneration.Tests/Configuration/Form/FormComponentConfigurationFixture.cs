using System;
using System.Linq;
using Enigmatry.BuildingBlocks.Validation;
using Enigmatry.CodeGeneration.Configuration.Form;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Tests.Configuration.Form
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
            componentModel.ComponentInfo.Feature.Name.Should().Be("Projects");

            componentModel.FormControls.Count.Should().Be(5);

            var titleFormControl = componentModel.FormControls
                .Single(x => x.PropertyName == nameof(ProjectDetails.Title).ToLowerInvariant());
            titleFormControl.ValidationRules.Count.Should().Be(1);
            titleFormControl.ValidationRules.Single().Name.Should().Be("maxLength");
            var detailsFormControl = componentModel.FormControls
                .Single(x => x.PropertyName == nameof(ProjectDetails.Description).ToLowerInvariant());
            detailsFormControl.ValidationRules.Count.Should().Be(1);
            detailsFormControl.ValidationRules.Single().Name.Should().Be("maxLength");

        }

        [UsedImplicitly]
        private class ProjectDetails
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = String.Empty;
            public string Description { get; set; } = String.Empty;
            public DateTimeOffset StartDate { get; set; }
            public DateTimeOffset EndDate { get; set; }
        }

        [UsedImplicitly]
        private class ProjectDetailsValidation : ValidationConfiguration<ProjectDetails>
        {
            public ProjectDetailsValidation()
            {
                RuleFor(x => x.Title)
                    .HasMaxLength(25, "Title is too long");
                RuleFor(x => x.Description)
                    .HasMaxLength(250, "Description is too long");
            }
        }

        private class Configuration : IFormComponentConfiguration<ProjectDetails>
        {
            public void Configure(FormComponentBuilder<ProjectDetails> builder)
            {
                builder.Component()
                    .HasName("ProjectDetails")
                    .BelongsToFeature("Projects");

                builder.FormControl(x => x.Id).IsVisible(false);

                builder.FormControl(x => x.Title)
                    .WithLabel("Title")
                    .WithPlaceholder("Title");

                builder.FormControl(x => x.StartDate)
                    .WithLabel("Start")
                    .WithPlaceholder("Start");

                builder.WithValidationConfiguration(new ProjectDetailsValidation());            }
        }
    }
}
