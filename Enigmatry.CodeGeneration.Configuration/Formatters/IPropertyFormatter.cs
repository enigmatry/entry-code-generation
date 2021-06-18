using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.CodeGeneration.Configuration.Formatters
{
    public interface IPropertyFormatter
    {
        public bool ValidateInputType(Type inputType) => SupportedInputTypes().Any(x => x.Name == inputType.Name)
            ? true
            : throw new ArgumentOutOfRangeException($"Property of type {inputType} is not compatible with property formatter.");

        IList<Type> SupportedInputTypes();

        public string JsFormatterName { get; }

        public string ToJsObject();
    }
}
