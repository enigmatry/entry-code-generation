using System;
using System.Diagnostics.CodeAnalysis;
using Enigmatry.CodeGeneration.Configuration.Form.Model;

namespace Enigmatry.CodeGeneration.Configuration
{
    public static class Check
    {
        public static T NotNull<T>(T value, [NotNull] string parameterName)
        {
            return value is null ? throw new ArgumentNullException(parameterName) : value;
        }

        public static string NotEmpty(string? value, [NotNull] string parameterName)
        {
            return String.IsNullOrEmpty(value) ? throw new ArgumentNullException(parameterName) : value;
        }

        public static FormControlBuilder IsSelectFormControl(FormControlBuilder value)
        {
            if (value.FormControlType != FormControlType.DropDownList && value.FormControlType != FormControlType.Autocomplete)
            {
                throw new ArgumentException(value.PropertyInfo.Name);
            }
            return value;
        }
    }
}
