using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

public class DecimalPropertyFormatter : BasePropertyFormatter
{
    public string DigitsInfo { get; private set; } = String.Empty;
    public string Locale { get; private set; } = String.Empty;
    public override string JsFormatterName => "number";

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

    public DecimalPropertyFormatter WithDigitsInfo(string value)
    {
        DigitsInfo = value;
        return this;
    }

    public DecimalPropertyFormatter WithLocale(string value)
    {
        Locale = value;
        return this;
    }

    public override string ToJsObject()
    {
        return DigitsInfo.HasContent() ? $"{{ name: \'{JsFormatterName}\', digitsInfo: \'{DigitsInfo}\', locale: \'{Locale}\' }}" : $"{{ name: \'{JsFormatterName}\' }}";
    }
}