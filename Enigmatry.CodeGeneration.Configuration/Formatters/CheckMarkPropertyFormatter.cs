using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Formatters
{
    public class CheckMarkPropertyFormatter : IPropertyFormatter
    {
        public IList<Type> SupportedInputTypes() => new List<Type> {typeof(bool)};
    }
}
