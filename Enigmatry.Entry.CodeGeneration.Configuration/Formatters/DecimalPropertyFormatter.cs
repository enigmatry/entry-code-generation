namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

/// <summary>
/// The DecimalPropertyFormatter is an implementation of the <see cref="IPropertyFormatter"/> interface
/// that formats numeric values as decimal numbers in a table column.
/// </summary>
/// <remarks>
/// This formatter uses the Angular number pipe under the hood.
/// </remarks>
public class DecimalPropertyFormatter : BasePropertyFormatter
{
    /// <summary>
    /// The digits information used for formatting.
    /// </summary>
    public string DigitsInfo { get; private set; } = String.Empty;

    /// <summary>
    /// The locale used for formatting.
    /// </summary>
    public string Locale { get; private set; } = String.Empty;

    /// <inheritdoc/>
    public override string JsFormatterName => "number";

    /// <inheritdoc/>
    public override IList<Type> SupportedInputTypes() =>
        new List<Type>
        {
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(string)
        };

    /// <summary>
    /// Sets the digits information used for formatting.
    /// </summary>
    public DecimalPropertyFormatter WithDigitsInfo(string value)
    {
        DigitsInfo = value;
        return this;
    }

    /// <summary>
    /// Sets the locale used for formatting.
    /// </summary>
    public DecimalPropertyFormatter WithLocale(string value)
    {
        Locale = value;
        return this;
    }

    /// <inheritdoc/>
    public override string ToJsObject()
    {
        return DigitsInfo.HasContent() ? $"{{ name: \'{JsFormatterName}\', digitsInfo: \'{DigitsInfo}\', locale: \'{Locale}\' }}" : $"{{ name: \'{JsFormatterName}\' }}";
    }
}
