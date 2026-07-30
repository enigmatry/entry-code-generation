using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

[Category("unit")]
internal sealed class AngularSignalsFormatterDefinitionFixture
{
    private sealed class ProbeCurrencyFormatter : CurrencyPropertyFormatter
    {
        public override string ToJsObject() => "{ name: 'probeCurrency', style: 'fancy' }";
    }

    [Test]
    public void SubclassOfBuiltInFormatterKeepsItsOwnDefinition()
    {
        var definition = new ProbeCurrencyFormatter().SignalsFormatterJsObject();

        definition.ShouldBe("{ name: 'probeCurrency', style: 'fancy' }");
    }

    [Test]
    public void ConfiguredValuesAreTsEscaped()
    {
        var formatter = new CurrencyPropertyFormatter().WithCurrencyCode("EUR").WithLocale("it's");

        var definition = formatter.SignalsFormatterJsObject();

        definition.ShouldContain(@"locale: 'it\'s'");
    }

    [TestCase(typeof(DatePropertyFormatter), "{ name: 'date' }")]
    [TestCase(typeof(CurrencyPropertyFormatter), "{ name: 'currency' }")]
    [TestCase(typeof(DecimalPropertyFormatter), "{ name: 'number' }")]
    [TestCase(typeof(PercentPropertyFormatter), "{ name: 'percent' }")]
    [TestCase(typeof(BooleanPropertyFormatter), "{ name: 'boolean' }")]
    public void UnconfiguredBuiltInFormatterEmitsNameOnly(Type formatterType, string expected)
    {
        var formatter = (IPropertyFormatter)Activator.CreateInstance(formatterType)!;

        var definition = formatter.SignalsFormatterJsObject();

        definition.ShouldBe(expected);
    }
}
