﻿using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Formatters
{
    public class PercentPropertyFormatter : BasePropertyFormatter
    {
        public string DigitsInfo { get; private set; } = String.Empty;
        public string Locale { get; private set; } = String.Empty;
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

        public override string ToJsObject()
        {
            return DigitsInfo.HasContent() ? $"{{ digitsInfo: \'{DigitsInfo}\', locale: \'{Locale}\' }}" : "undefined";
        }
    }
}
