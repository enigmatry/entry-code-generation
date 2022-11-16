using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters
{
    public class NoFormattingPropertyFormatter : BasePropertyFormatter
    {
        public override IList<Type> SupportedInputTypes() => new List<Type>();

        public override string ToJsObject() => "undefined";

        public override string JsFormatterName => String.Empty;

        public override bool ValidateInputType(Type inputType) => true;
    }
}
