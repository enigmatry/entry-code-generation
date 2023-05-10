using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

/// <summary>
/// The PercentPropertyFormatter is an implementation of the <see cref="IPropertyFormatter"/> interface
/// that formats numerical property values as percentages in a table column.
/// This formatter can be configured with a custom number of digits, locale, and an optional multiplier.
/// </summary>
public class PercentPropertyFormatter : BasePropertyFormatter
{
    /// <summary>
    /// The number of digits to display after the decimal point.
    /// </summary>
    public string DigitsInfo { get; private set; } = String.Empty;

    /// <summary>
    /// The locale to use for formatting.
    /// </summary>
    public string Locale { get; private set; } = String.Empty;

    /// <summary>
    /// The multiplier value to apply.
    /// </summary>
    public decimal? Multiplier { get; private set; }
    public override string JsFormatterName => "percent";

    public override IList<Type> SupportedInputTypes() =>
        new List<Type>
        {
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal)
        };

    /// <summary>
    /// Configures the digits info.
    /// </summary>
    /// <param name="value">The number of digits to display.</param>
    /// <returns>The current instance of <see cref="PercentPropertyFormatter"/>.</returns>
    public PercentPropertyFormatter WithDigitsInfo(string value)
    {
        DigitsInfo = value;
        return this;
    }

    /// <summary>
    /// Configures the locale to use for formatting the value.
    /// </summary>
    /// <param name="value">The locale to use for formatting.</param>
    /// <returns>The current instance of <see cref="PercentPropertyFormatter"/>.</returns>
    public PercentPropertyFormatter WithLocale(string value)
    {
        Locale = value;
        return this;
    }

    /// <summary>
    /// Configures an optional multiplier for the value.
    /// </summary>
    /// <param name="value">The multiplier value to apply.</param>
    /// <returns>The current instance of <see cref="PercentPropertyFormatter"/>.</returns>
    public PercentPropertyFormatter WithMultiplier(decimal value)
    {
        Multiplier = value;
        return this;
    }

    public override string ToJsObject()
    {
        var multiplier = Multiplier.HasValue ? $", multiplier: \'{Multiplier}\'" : String.Empty;
        return DigitsInfo.HasContent() ? 
            $"{{ name: \'{JsFormatterName}\', digitsInfo: \'{DigitsInfo}\', locale: \'{Locale}\'{multiplier} }}" : 
            $"{{ name: \'{JsFormatterName}\'{multiplier} }}";
    }
}
