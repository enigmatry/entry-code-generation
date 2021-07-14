using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Formatters
{
    public interface IPropertyFormatter
    {
        public bool ValidateInputType(Type inputType);

        IList<Type> SupportedInputTypes();

        public string JsFormatterName { get; }

        public string ToJsObject();
    }
}
