using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Formatters
{
    public class NoFormattingPropertyFormatter : BasePropertyFormatter
    {
        public override IList<Type> SupportedInputTypes() => new List<Type>();

        public override string ToJsObject() => "undefined";

        public override string JsFormatterName => String.Empty;

        public new bool ValidateInputType(Type inputType) => true;
    }
}
