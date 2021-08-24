using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.Form.Model;

namespace Enigmatry.CodeGeneration.Configuration
{
    public static class Check
    {
        public static T NotNull<T>(T value, string parameterName)
        {
            return value is null ? throw new ArgumentNullException(parameterName) : value;
        }

        public static string NotEmpty(string? value, string parameterName)
        {
            return (value == null || String.IsNullOrEmpty(value))
                ? throw new ArgumentNullException(parameterName)
                : value;
        }

        public static FormControlBuilder IsSelectFormControl(FormControlBuilder value)
        {
            if (value.FormControlType != FormControlType.Select && value.FormControlType != FormControlType.Autocomplete)
            {
                throw new ArgumentException(value.PropertyInfo.Name);
            }
            return value;
        }

        public static bool IsNumber(Type type) =>
            new List<Type>
            {
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(decimal),
                typeof(double),
                typeof(float),
                typeof(byte)
            }
            .Any(x => x == type);
    }
}
