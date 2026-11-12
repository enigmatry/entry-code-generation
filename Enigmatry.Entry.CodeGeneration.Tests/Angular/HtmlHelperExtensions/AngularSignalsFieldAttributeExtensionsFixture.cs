using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

[Category("unit")]
internal sealed class AngularSignalsFieldAttributeExtensionsFixture
{
    private sealed class CustomFormatter : BasePropertyFormatter
    {
        public override string JsFormatterName => "custom";

        public override IList<Type> SupportedInputTypes() => [typeof(decimal)];

        public override string ToJsObject() => "{ name: 'custom' }";
    }

    private sealed class RenamedCurrencyFormatter : CurrencyPropertyFormatter
    {
        public override string ToJsObject() => "{ name: 'probeCurrency' }";
    }

    private sealed class InheritedCurrencyFormatter : CurrencyPropertyFormatter
    {
    }

    [TestCase(typeof(DecimalPropertyFormatter))]
    [TestCase(typeof(CurrencyPropertyFormatter))]
    [TestCase(typeof(PercentPropertyFormatter))]
    [TestCase(typeof(InheritedCurrencyFormatter))]
    public void NumberDefinitionShowsFormattedNumber(Type formatterType)
    {
        var field = new InputFormControl { PropertyType = typeof(decimal), Formatter = (IPropertyFormatter)Activator.CreateInstance(formatterType)! };

        field.ShowsFormattedNumber().ShouldBeTrue();
    }

    [TestCase(typeof(DatePropertyFormatter))]
    [TestCase(typeof(BooleanPropertyFormatter))]
    [TestCase(typeof(NoFormattingPropertyFormatter))]
    [TestCase(typeof(CustomFormatter))]
    [TestCase(typeof(RenamedCurrencyFormatter))]
    public void OtherDefinitionDoesNotShowFormattedNumber(Type formatterType)
    {
        var field = new InputFormControl { PropertyType = typeof(decimal), Formatter = (IPropertyFormatter)Activator.CreateInstance(formatterType)! };

        field.ShowsFormattedNumber().ShouldBeFalse();
    }

    [Test]
    public void FieldWithoutFormatDoesNotShowFormattedNumber()
    {
        var field = new InputFormControl { PropertyType = typeof(int) };

        field.ShowsFormattedNumber().ShouldBeFalse();
    }

    [Test]
    public void FixedInputTypeWinsOverNumberFormat()
    {
        var field = new EmailFormControl { PropertyType = typeof(string), Formatter = new CurrencyPropertyFormatter() };

        field.ShowsFormattedNumber().ShouldBeFalse();
    }
}
