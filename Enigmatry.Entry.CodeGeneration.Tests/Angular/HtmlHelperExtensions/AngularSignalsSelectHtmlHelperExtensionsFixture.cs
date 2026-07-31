using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

internal sealed class AngularSignalsSelectHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
{
    private static readonly SelectOption[] GroupedOptions =
    [
        new SelectOption("nl", "Netherlands", "country.nl") { Group = new I18NString("group.eu", "Europe") },
        new SelectOption("rs", "Serbia", "country.rs") { Group = new I18NString("group.eu", "Europe") },
        new SelectOption("us", "United States", "country.us")
    ];

    private string GroupedSelectDeclarations()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.SelectFormControl(x => x.Region)
            .WithOptions(options => options.WithFixedValues(GroupedOptions));
        var select = builder.Build().FormControls.OfType<SelectFormControl>().Single(control => control.PropertyName == "region");

        return _htmlHelper.SelectInputDeclarations(select, false).ToString() ?? "";
    }

    [Test]
    public void GroupedFixedOptionsCarryTheirGroup()
    {
        var declarations = GroupedSelectDeclarations();

        declarations.ShouldContain("value: 'nl', displayName: `Netherlands`, group: `Europe`");
        declarations.ShouldContain("value: 'us', displayName: `United States` }");
    }

    [Test]
    public void GroupedSelectDeclaresOptionGroups()
    {
        var declarations = GroupedSelectDeclarations();

        declarations.ShouldContain("regionOptionGroups = computed(");
        declarations.ShouldContain("group: option[configuration.groupProperty ?? 'group']");
    }

    [Test]
    public void GroupKeyIsPassedToConfigurationAndSortOptions()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.SelectFormControl(x => x.CategoryId)
            .WithOptions(options =>
            {
                options.WithDynamicValues();
                options.WithValueKey("id");
                options.WithDisplayKey("categoryName");
                options.WithGroupKey("categoryGroup");
            });
        var select = builder.Build().FormControls.OfType<SelectFormControl>().Single(control => control.PropertyName == "categoryId");

        var declarations = _htmlHelper.SelectInputDeclarations(select, false).ToString() ?? "";

        declarations.ShouldContain("groupProperty: 'categoryGroup'");
        declarations.ShouldContain("configuration.sortProperty ?? '', this.localeId, configuration.groupProperty)");
    }

    [Test]
    public void UngroupedSelectDeclaresNoGroupingMembers()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.SelectFormControl(x => x.Region)
            .WithOptions(options => options.WithFixedValues(new[] { new SelectOption("nl", "Netherlands", "country.nl") }));
        var select = builder.Build().FormControls.OfType<SelectFormControl>().Single(control => control.PropertyName == "region");

        var declarations = _htmlHelper.SelectInputDeclarations(select, false).ToString() ?? "";

        declarations.ShouldNotContain("OptionGroups");
        declarations.ShouldNotContain("groupProperty");
    }

    [Test]
    public void MultiCheckboxWithGroupedOptionsDeclaresNoOptionGroups()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.MultiCheckboxFormControl(x => x.MultiCheckboxWithStringIds)
            .WithOptions(options => options.WithFixedValues(GroupedOptions));
        var multiCheckbox = builder.Build().FormControls.OfType<MultiCheckboxFormControl>().Single();

        var declarations = _htmlHelper.SelectInputDeclarations(multiCheckbox, false).ToString() ?? "";

        declarations.ShouldContain("group: `Europe`");
        declarations.ShouldNotContain("OptionGroups");
    }
}
