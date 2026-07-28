using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Maps the statically-known property formatters to the equivalent Angular pipes so the
/// readonly display matches the configured (Formly-era typeFormatDef) presentation.
/// </summary>
internal static class AngularSignalsFormatterPipes
{
    internal static string? PipeSymbol(this IPropertyFormatter formatter) => formatter.JsFormatterName switch
    {
        "date" => "DatePipe",
        "currency" => "CurrencyPipe",
        "number" => "DecimalPipe",
        "percent" => "PercentPipe",
        _ => null
    };

    /// <summary>Pipe expression such as "currency:'EUR':'symbol'" — positional arguments trimmed to the last configured one, gaps emitted as undefined. Null when no pipe matches (e.g. boolean).</summary>
    internal static string? PipeExpression(this IPropertyFormatter formatter)
    {
        var arguments = formatter switch
        {
            DatePropertyFormatter date => new[] { date.Format, date.TimeZone, date.Locale },
            CurrencyPropertyFormatter currency => new[] { currency.CurrencyCode, currency.Display, currency.DigitsInfo, currency.Locale },
            DecimalPropertyFormatter number => new[] { number.DigitsInfo, number.Locale },
            PercentPropertyFormatter percent => new[] { percent.DigitsInfo, percent.Locale },
            _ => null
        };

        var pipeName = formatter.JsFormatterName;
        if (arguments == null || formatter.PipeSymbol() == null)
        {
            return null;
        }

        var lastConfiguredIndex = Array.FindLastIndex(arguments, argument => argument.HasContent());
        var argumentList = String.Concat(arguments.Take(lastConfiguredIndex + 1)
            .Select(argument => argument.HasContent() ? $":'{argument.EscapeTsSingleQuoted()}'" : ":undefined"));

        return $"{pipeName}{argumentList}";
    }
}
