using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

internal sealed class AngularSignalsArrayRowModelHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
{
    private static FormComponentModel BuildModelWithAddressArray()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.ArrayFormControl(x => x.Addresses)
            .WithItemConfiguration(itemConfiguration => itemConfiguration.InputFormControl(x => x.City));
        return builder.Build();
    }

    [Test]
    public void SubmitMergeReadsTrackedOriginalRows()
    {
        var mergeProperties = _htmlHelper.ArrayRowMergeProperties(BuildModelWithAddressArray()).ToString() ?? "";

        mergeProperties.ShouldContain("this.addressesOriginalRows[index]");
        mergeProperties.ShouldNotContain("this.model().addresses");
    }

    [Test]
    public void RowModelMergesOriginalRowUnderCurrentValues()
    {
        var declarations = _htmlHelper.ArrayOriginalRowsDeclarations(BuildModelWithAddressArray()).ToString() ?? "";

        declarations.ShouldContain("private addressesOriginalRows: object[] = [];");
        declarations.ShouldContain("...(this.addressesOriginalRows[index] ?? {}), ...(this.currentModel().addresses?.[index] ?? {})");
    }

    [Test]
    public void MutationMethodsKeepOriginalRowsAligned()
    {
        var arrayControl = BuildModelWithAddressArray().FormControls.OfType<ArrayFormControl>().Single();

        var mutationMethods = _htmlHelper.ArrayMutationMethods(arrayControl).ToString() ?? "";

        mutationMethods.ShouldContain("this.addressesOriginalRows = [...this.addressesOriginalRows, {}];");
        mutationMethods.ShouldContain("this.addressesOriginalRows.filter((_, originalRowIndex) => originalRowIndex !== index)");
    }
}
