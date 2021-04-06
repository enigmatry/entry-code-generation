using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Formatters
{
    public interface IPropertyFromatter
    {
        public bool ValidateInputType(Type inputType) => SupportedInputTypes().Any(x => x.Name == inputType.Name)
            ? true
            : throw new ArgumentOutOfRangeException($"Property of type {inputType} is not compatible with property formatter.");

        IList<Type> SupportedInputTypes();
    }
}
