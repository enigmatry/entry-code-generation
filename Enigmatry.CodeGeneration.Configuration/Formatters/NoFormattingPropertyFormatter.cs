using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Formatters
{
    public class NoFormattingPropertyFormatter : IPropertyFormatter
    {
        public IList<Type> SupportedInputTypes() => new List<Type>();

        public string ToJsObject() => "undefined";

        public string JsFormatterName => String.Empty;

        public bool ValidateInputType(Type inputType) => true;
    }
}
