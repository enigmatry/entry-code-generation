using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters
{
    public class PercentPropertyFormatter : BasePropertyFormatter
    {
        public string DigitsInfo { get; private set; } = String.Empty;
        public string Locale { get; private set; } = String.Empty;
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

        public PercentPropertyFormatter WithDigitsInfo(string value)
        {
            DigitsInfo = value;
            return this;
        }

        public PercentPropertyFormatter WithLocale(string value)
        {
            Locale = value;
            return this;
        }

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
}
