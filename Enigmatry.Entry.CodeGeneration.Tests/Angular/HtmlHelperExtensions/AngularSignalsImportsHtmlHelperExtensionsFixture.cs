using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

[Category("unit")]
internal sealed class AngularSignalsImportsHtmlHelperExtensionsFixture
{
    [Test]
    public void SymbolsFromEntryFormPackageAreEmitted()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.InputFormControl(x => x.Name).WithImport("EntryNameComponent", "@enigmatry/entry-form");
        builder.InputFormControl(x => x.Description).WithImport("EntryOtherComponent", "@enigmatry/entry-other");

        var symbols = builder.Build().EntryFormImportSymbols();

        symbols.ShouldBe(", EntryNameComponent");
    }

    [Test]
    public void FormatDirectiveAlreadyMergedIsSkipped()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.InputFormControl(x => x.Money)
            .WithFormat(new CurrencyPropertyFormatter())
            .WithImport("EntryFieldFormatDirective", "@enigmatry/entry-form");

        var symbols = builder.Build().EntryFormImportSymbols();

        symbols.ShouldBeEmpty();
    }
}
