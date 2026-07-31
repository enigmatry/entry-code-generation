using Enigmatry.Entry.CodeGeneration.Angular;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular;

[Category("unit")]
internal sealed class SignalsFormComponentValidatorFixture
{
    [Test]
    public void RichTextWithoutImportThrows()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.RichTextInputFormControl(x => x.Description);

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("description");
        exception.Message.ShouldContain("WithImport");
    }

    [Test]
    public void CustomControlWithoutImportThrows()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.CustomFormControl(x => x.FileUpload).WithCustomControlType("entry-file-input");

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("fileUpload");
        exception.Message.ShouldContain("WithImport");
    }

    [Test]
    public void AutocompleteInsideArrayItemThrows()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.ArrayFormControl(x => x.Addresses)
            .WithItemConfiguration(itemConfiguration => itemConfiguration.AutocompleteFormControl(x => x.City));

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("addresses.city");
        exception.Message.ShouldContain("not supported inside array items");
    }

    [Test]
    public void NestedArrayInsideArrayItemThrows()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.ArrayFormControl(x => x.Addresses)
            .WithItemConfiguration(itemConfiguration =>
                itemConfiguration.ArrayFormControl(x => x.NestedAddresses).WithItemConfiguration(_ => { }));

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("addresses");
        exception.Message.ShouldContain("Nested arrays are not supported");
    }

    [Test]
    public void CollidingSelectMemberNamesThrow()
    {
        var builder = new FormComponentBuilder<FormCollisionMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.SelectFormControl(x => x.AddressesCountry);
        builder.ArrayFormControl(x => x.Addresses)
            .WithItemConfiguration(itemConfiguration => itemConfiguration.SelectFormControl(x => x.Country));

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("addressesCountry");
        exception.Message.ShouldContain("addresses.country");
        exception.Message.ShouldContain("colliding member names");
    }

    [Test]
    public void MemberNameCollidingWithAnotherControlsSuffixedMemberThrows()
    {
        var builder = new FormComponentBuilder<FormCollisionMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.AutocompleteFormControl(x => x.Region);
        builder.SelectFormControl(x => x.RegionFiltered);

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("regionFilteredOptions");
        exception.Message.ShouldContain("region");
        exception.Message.ShouldContain("regionFiltered");
    }

    [Test]
    public void GroupingMembersAreRegisteredForCollisionChecking()
    {
        var builder = new FormComponentBuilder<FormCollisionMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.AutocompleteFormControl(x => x.Region)
            .WithOptions(options => options.WithFixedValues(new[]
            {
                new SelectOption("nl", "Netherlands", "country.nl") { Group = new I18NString("group.eu", "Europe") }
            }));
        var autocomplete = builder.Build().FormControls.OfType<AutocompleteFormControl>().Single();

        var memberNames = autocomplete.GeneratedMemberNames("").ToList();

        memberNames.ShouldContain("regionFilteredOptionGroups");
        memberNames.ShouldNotContain("regionOptionGroups");
    }

    [Test]
    public void RuleWithoutRuleValueTemplateOptionWarns()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.InputFormControl(x => x.Name);
        var model = builder.Build();
        model.FormControls.Single(control => control.PropertyName == "name").ValidationRules.Add(new UnsupportedValidationRuleMock());

        var warnings = SignalsFormComponentValidator.Validate(model);

        var warning = warnings.ShouldHaveSingleItem();
        warning.ShouldContain("nameCheck");
        warning.ShouldContain("MockEdit.name");
        warning.ShouldContain("ENTRY_ASYNC_VALIDATOR_RESOLVER");
    }

    [Test]
    public void MockConfigurationPassesWithoutWarnings()
    {
        var builder = new FormComponentBuilder<FormMock>();
        new FormMockConfiguration().Configure(builder);

        var warnings = SignalsFormComponentValidator.Validate(builder.Build());

        warnings.ShouldBeEmpty();
    }
}
