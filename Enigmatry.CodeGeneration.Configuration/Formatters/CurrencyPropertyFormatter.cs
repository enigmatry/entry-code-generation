using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Formatters
{
    public class CurrencyPropertyFormatter : BasePropertyFormatter
    {
        public string CurrencyCode { get; private set; } = String.Empty;
        public string Display { get; private set; } = String.Empty;
        public string DigitsInfo { get; private set; } = String.Empty;
        public string Locale { get; private set; } = String.Empty;
        public override string JsFormatterName => "currency";

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

        public CurrencyPropertyFormatter WithCurrencyCode(string value)
        {
            CurrencyCode = value;
            return this;
        }

        public CurrencyPropertyFormatter WithDisplay(string value)
        {
            Display = value;
            return this;
        }

        public CurrencyPropertyFormatter WithDigitsInfo(string value)
        {
            DigitsInfo = value;
            return this;
        }

        public CurrencyPropertyFormatter WithLocale(string value)
        {
            Locale = value;
            return this;
        }

        public override string ToJsObject()
        {
            return CurrencyCode.HasContent()
                ? $"{{ currencyCode: \'{CurrencyCode}\', display: \'{Display}\', digitsInfo: \'{DigitsInfo}\', locale: \'{Locale}\' }}"
                : "undefined";
        }
    }
}
