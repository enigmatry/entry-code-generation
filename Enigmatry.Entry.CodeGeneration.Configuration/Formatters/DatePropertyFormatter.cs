using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

/// <summary>
/// The DatePropertyFormatter is an implementation of the <see cref="IPropertyFormatter"/> interface
/// that formats DateTime, DateTimeOffset, and DateOnly property values as dates in a table column.
/// This formatter uses the Angular DatePipe to display the formatted date.
/// </summary>
public class DatePropertyFormatter : BasePropertyFormatter
{
    /// <summary>
    /// The format string to be used for formatting the date.
    /// </summary>
    public string Format { get; private set; } = String.Empty;

    /// <summary>
    /// The time zone to be used for formatting the date.
    /// </summary>
    public string TimeZone { get; private set; } = String.Empty;

    /// <summary>
    /// The locale to be used for formatting the date.
    /// </summary>
    public string Locale { get; private set; } = String.Empty;
    public override string JsFormatterName => "date";

    public override IList<Type> SupportedInputTypes() => new List<Type>
    {
        typeof(DateTime), 
        typeof(DateTimeOffset),
#if NET6_0
        typeof(DateOnly)
#endif
    };

    /// <summary>
    /// Set the format for the date property formatter.
    /// </summary>
    /// <param name="value">The format string to be used for formatting the date.</param>
    /// <returns>The <see cref="DatePropertyFormatter"/> instance for method chaining.</returns>
    public DatePropertyFormatter WithFormat(string value)
    {
        Format = value;
        return this;
    }

    /// <summary>
    /// Set the time zone for the date property formatter.
    /// </summary>
    /// <param name="value">The time zone to be used for formatting the date.</param>
    /// <returns>The <see cref="DatePropertyFormatter"/> instance for method chaining.</returns>
    public DatePropertyFormatter WithTimeZone(string value)
    {
        TimeZone = value;
        return this;
    }

    /// <summary>
    /// Set the locale for the date property formatter.
    /// </summary>
    /// <param name="value">The locale to be used for formatting the date.</param>
    /// <returns>The <see cref="DatePropertyFormatter"/> instance for method chaining.</returns>
    public DatePropertyFormatter WithLocale(string value)
    {
        Locale = value;
        return this;
    }

    public override string ToJsObject()
    {
        return Format.HasContent() ? $"{{ name: \'{JsFormatterName}\', format: \'{Format}\', timezone: \'{TimeZone}\', locale: \'{Locale}\' }}" : $"{{ name: \'{JsFormatterName}\' }}";
    }
}
