using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters
{
    public class DatePropertyFormatter : BasePropertyFormatter
    {
        public string Format { get; private set; } = String.Empty;
        public string TimeZone { get; private set; } = String.Empty;
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

        public DatePropertyFormatter WithFormat(string value)
        {
            Format = value;
            return this;
        }

        public DatePropertyFormatter WithTimeZone(string value)
        {
            TimeZone = value;
            return this;
        }

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
}
