using System.Globalization;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Signals-side mirror of <see cref="IPropertyFormatter.ToJsObject"/> with TS string escaping of
/// the consumer-configured values; the shared implementations stay untouched because the
/// deprecated Formly templates rely on their exact output. Unknown (consumer-provided) formatter
/// types fall back to their own ToJsObject.
/// </summary>
internal static class AngularSignalsFormatterDefinition
{
    internal static string SignalsFormatterJsObject(this IPropertyFormatter formatter) => formatter switch
    {
        DatePropertyFormatter date => date.Format.HasContent()
            ? $"{{ name: 'date', format: {Quote(date.Format)}, timezone: {Quote(date.TimeZone)}, locale: {Quote(date.Locale)} }}"
            : "{ name: 'date' }",
        CurrencyPropertyFormatter currency => currency.CurrencyCode.HasContent()
            ? $"{{ name: 'currency', currencyCode: {Quote(currency.CurrencyCode)}, display: {Quote(currency.Display)}, digitsInfo: {Quote(currency.DigitsInfo)}, locale: {Quote(currency.Locale)} }}"
            : "{ name: 'currency' }",
        DecimalPropertyFormatter number => number.DigitsInfo.HasContent()
            ? $"{{ name: 'number', digitsInfo: {Quote(number.DigitsInfo)}, locale: {Quote(number.Locale)} }}"
            : "{ name: 'number' }",
        PercentPropertyFormatter percent => percent.DigitsInfo.HasContent()
            ? $"{{ name: 'percent', digitsInfo: {Quote(percent.DigitsInfo)}, locale: {Quote(percent.Locale)}{MultiplierProperty(percent)} }}"
            : $"{{ name: 'percent'{MultiplierProperty(percent)} }}",
        BooleanPropertyFormatter => "{ name: 'boolean' }",
        _ => formatter.ToJsObject()
    };

    private static string MultiplierProperty(PercentPropertyFormatter percent) => percent.Multiplier.HasValue
        ? $", multiplier: '{percent.Multiplier.Value.ToString(CultureInfo.InvariantCulture)}'"
        : "";

    private static string Quote(string value) => $"'{value.EscapeTsSingleQuoted()}'";
}
