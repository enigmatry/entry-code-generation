using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

internal sealed class AngularSignalsControlStateHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
{
    [Test]
    public void GroupReadonlyPropagatesToChildState()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        var group = builder.FormControlGroup("Details");
        group.IsReadonly(true);
        group.InputFormControl(x => x.Name);

        var declarations = _htmlHelper.ControlStateDeclarations(builder.Build()).ToString() ?? "";

        declarations.ShouldContain("{ key: 'name', staticVisible: true, staticReadonly: true }");
    }

    [Test]
    public void ArrayWithoutStatefulChildrenEmitsNoItemStateTable()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.ArrayFormControl(x => x.Addresses)
            .WithItemConfiguration(itemConfiguration => itemConfiguration.ButtonFormControl("RowBtn").WithText("Row"));

        var model = builder.Build();

        (_htmlHelper.ControlStateDeclarations(model).ToString() ?? "").ShouldNotContain("addressesItemControlStates");
        (_htmlHelper.ArrayItemControlStateLines(model).ToString() ?? "").ShouldBeEmpty();
    }

    [Test]
    public void ArrayChildStateHonorsDisableExpressions()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.ArrayFormControl(x => x.Addresses)
            .WithItemConfiguration(itemConfiguration => itemConfiguration.InputFormControl(x => x.City));

        var stateLines = _htmlHelper.ArrayItemControlStateLines(builder.Build()).ToString() ?? "";

        stateLines.ShouldContain("this.isDisabled(key, staticReadonly, rowModel)");
    }
}
