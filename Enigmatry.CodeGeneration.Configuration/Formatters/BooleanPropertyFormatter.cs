using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Formatters
{
    public class BooleanPropertyFormatter : IPropertyFormatter
    {
        public IList<Type> SupportedInputTypes() => new List<Type> {typeof(bool)};

        public string JsFormatterName => "boolean";

        public string ToJsObject() => "undefined";
    }
}
