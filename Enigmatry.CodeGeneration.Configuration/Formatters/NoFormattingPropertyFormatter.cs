using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Formatters
{
    public class NoFormattingPropertyFormatter : IPropertyFromatter
    {
        public IList<Type> SupportedInputTypes() => new List<Type>();

        public bool ValidateInputType(Type inputType) => true;
    }
}
