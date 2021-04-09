using Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public static class Check
    {
        public static T NotNull<T>(T value, [NotNull] string parameterName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        public static string NotEmpty(string? value, [NotNull] string parameterName)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
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
