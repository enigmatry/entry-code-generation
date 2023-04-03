using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

/// <summary>
/// The CurrencyPropertyFormatter formats numeric values as currency values in a table column.
/// </summary>
/// <remarks>
/// This formatter uses the Angular currency pipe under the hood.
/// </remarks>
public class CurrencyPropertyFormatter : BasePropertyFormatter
{
    /// <summary>
    /// The currency code (e.g. 'USD', 'EUR') used for formatting.
    /// </summary>
    public string CurrencyCode { get; private set; } = String.Empty;

    /// <summary>
    /// The display format of the currency value.
    /// </summary>
    public string Display { get; private set; } = String.Empty;
    
    /// <summary>
    /// The digits information used for formatting.
    /// </summary>
    public string DigitsInfo { get; private set; } = String.Empty;
    
    /// <summary>
    /// The locale used for formatting.
    /// </summary>
    public string Locale { get; private set; } = String.Empty;
    
    /// <inheritdoc/>
    public override string JsFormatterName => "currency";
    
    /// <inheritdoc/>
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
    /// Sets the currency code (e.g. 'USD', 'EUR') used for formatting.
    /// </summary>
    public CurrencyPropertyFormatter WithCurrencyCode(string value)
    {
        CurrencyCode = value;
        return this;
    }

    /// <summary>
    /// Sets the display format of the currency value.
    /// </summary>
    public CurrencyPropertyFormatter WithDisplay(string value)
    {
        Display = value;
        return this;
    }

    /// <summary>
    /// Sets the digits information used for formatting.
    /// </summary>
    public CurrencyPropertyFormatter WithDigitsInfo(string value)
    {
        DigitsInfo = value;
        return this;
    }

    /// <summary>
    /// Sets the locale used for formatting.
    /// </summary>
    public CurrencyPropertyFormatter WithLocale(string value)
    {
        Locale = value;
        return this;
    }

    /// <inheritdoc/>
    public override string ToJsObject()
    {
        return CurrencyCode.HasContent()
            ? $"{{ name: \'{JsFormatterName}\', currencyCode: \'{CurrencyCode}\', display: \'{Display}\', digitsInfo: \'{DigitsInfo}\', locale: \'{Locale}\' }}"
            : $"{{ name: \'{JsFormatterName}\' }}";
    }
}
