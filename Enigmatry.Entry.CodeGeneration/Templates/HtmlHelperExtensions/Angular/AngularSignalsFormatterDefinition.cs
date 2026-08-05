using System.Globalization;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Signals-side mirror of <see cref="IPropertyFormatter.ToJsObject"/> with TS string escaping of
/// the consumer-configured values; the shared implementations stay untouched because the
/// deprecated Formly templates rely on their exact output. Only the exact built-in formatter types
/// are mirrored — every other formatter, including a consumer subclass of a built-in one, keeps
/// its own (possibly overridden) ToJsObject output.
/// </summary>
internal static class AngularSignalsFormatterDefinition
{
    internal static string SignalsFormatterJsObject(this IPropertyFormatter formatter) => formatter switch
    {
        DatePropertyFormatter date when date.IsExactly<DatePropertyFormatter>() => date.Format.HasContent()
            ? $"{{ name: 'date', format: {Quote(date.Format)}, timezone: {Quote(date.TimeZone)}, locale: {Quote(date.Locale)} }}"
            : "{ name: 'date' }",
        CurrencyPropertyFormatter currency when currency.IsExactly<CurrencyPropertyFormatter>() => currency.CurrencyCode.HasContent()
            ? $"{{ name: 'currency', currencyCode: {Quote(currency.CurrencyCode)}, display: {Quote(currency.Display)}, digitsInfo: {Quote(currency.DigitsInfo)}, locale: {Quote(currency.Locale)} }}"
            : "{ name: 'currency' }",
        DecimalPropertyFormatter number when number.IsExactly<DecimalPropertyFormatter>() => number.DigitsInfo.HasContent()
            ? $"{{ name: 'number', digitsInfo: {Quote(number.DigitsInfo)}, locale: {Quote(number.Locale)} }}"
            : "{ name: 'number' }",
        PercentPropertyFormatter percent when percent.IsExactly<PercentPropertyFormatter>() => percent.DigitsInfo.HasContent()
            ? $"{{ name: 'percent', digitsInfo: {Quote(percent.DigitsInfo)}, locale: {Quote(percent.Locale)}{MultiplierProperty(percent)} }}"
            : $"{{ name: 'percent'{MultiplierProperty(percent)} }}",
        BooleanPropertyFormatter booleanFormatter when booleanFormatter.IsExactly<BooleanPropertyFormatter>() => "{ name: 'boolean' }",
        _ => formatter.ToJsObject()
    };

    // A subclass may override ToJsObject/JsFormatterName, so mirroring it by base type would
    // silently replace its definition with the base formatter's.
    private static bool IsExactly<TFormatter>(this IPropertyFormatter formatter) => formatter.GetType() == typeof(TFormatter);

    private static string MultiplierProperty(PercentPropertyFormatter percent) => percent.Multiplier.HasValue
        ? $", multiplier: '{percent.Multiplier.Value.ToString(CultureInfo.InvariantCulture)}'"
        : "";

    private static string Quote(string value) => $"'{value.EscapeTsSingleQuoted()}'";
}
