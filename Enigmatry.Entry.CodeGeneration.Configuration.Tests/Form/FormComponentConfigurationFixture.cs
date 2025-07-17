using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Validation;
using JetBrains.Annotations;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests.Form;

[Category("unit")]
public class FormComponentConfigurationFixture
{
    private FormConfiguration _configuration = null!;
    private FormComponentBuilder<ProjectDetails> _builder = null!;

    [SetUp]
    public void Setup()
    {
        _configuration = new FormConfiguration();
        _builder = new FormComponentBuilder<ProjectDetails>();
    }

    [Test]
    public void TestFormComponentConfiguration()
    {
        _configuration.Configure(_builder);
        var formComponent = _builder.Build();

        formComponent.ShouldNotBeNull();
        formComponent.ComponentInfo.Name.ShouldBe("ProjectDetails");
        formComponent.ComponentInfo.Feature.Name.ShouldBe("Projects");

        formComponent.FormControls.Count.ShouldBe(5);

        var titleFormControl = formComponent.FormControls.Single(x => x.PropertyName == nameof(ProjectDetails.Title).ToLowerInvariant());

        titleFormControl.ValidationRules.Count.ShouldBe(1);
        titleFormControl.ValidationRules.Single()
            .FormlyRuleName
            .ShouldBe("maxLength");

        var detailsFormControl = formComponent.FormControls.Single(x => x.PropertyName == nameof(ProjectDetails.Description).ToLowerInvariant());
        detailsFormControl.ValidationRules.Count.ShouldBe(1);
        detailsFormControl.ValidationRules.Single()
            .FormlyRuleName
            .ShouldBe("maxLength");
    }

    [Test]
    public void TestExcludedUnconfiguredProperties()
    {
        _builder.Component().IncludeUnconfiguredProperties(false);

        _configuration.Configure(_builder);
        var formComponent = _builder.Build();

        formComponent.ShouldNotBeNull();
        formComponent.FormControls.Count.ShouldBe(4);
    }

    [Test]
    public void TestOrderByModel()
    {
        _builder.Component().OrderBy(OrderByType.Model);

        _configuration.Configure(_builder);
        var formComponent = _builder.Build();

        formComponent.ShouldNotBeNull();
        formComponent.FormControls.Count.ShouldBe(5);

        formComponent.FormControls.ElementAt(0).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.Id).ToLower());
        formComponent.FormControls.ElementAt(1).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.Title).ToLower());
        formComponent.FormControls.ElementAt(2).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.Description).ToLower());
        formComponent.FormControls.ElementAt(3).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.StartDate).ToLower());
        formComponent.FormControls.ElementAt(4).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.EndDate).ToLower());
    }

    [Test]
    public void TestOrderByConfiguration()
    {
        _builder.Component().OrderBy(OrderByType.Configuration);

        _configuration.Configure(_builder);
        var formComponent = _builder.Build();

        formComponent.ShouldNotBeNull();
        formComponent.FormControls.Count.ShouldBe(5);

        formComponent.FormControls.ElementAt(0).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.Id).ToLower());
        formComponent.FormControls.ElementAt(1).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.Title).ToLower());
        formComponent.FormControls.ElementAt(2).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.StartDate).ToLower());
        formComponent.FormControls.ElementAt(3).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.EndDate).ToLower());
        formComponent.FormControls.ElementAt(4).PropertyName.ToLower().ShouldBe(nameof(ProjectDetails.Description).ToLower());
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
                .MaxLength(25).WithMessage("Title is too long");
            RuleFor(x => x.Description)
                .MaxLength(250).WithMessage("Description is too long");
        }
    }

    private class FormConfiguration : IFormComponentConfiguration<ProjectDetails>
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

            builder.DatepickerFormControl(x => x.StartDate);
            builder.DatepickerFormControl(x => x.EndDate);

            builder.WithValidationConfiguration(new ProjectDetailsValidation());
        }
    }
}
