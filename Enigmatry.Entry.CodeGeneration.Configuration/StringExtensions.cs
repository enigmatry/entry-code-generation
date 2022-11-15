using System;

namespace Enigmatry.CodeGeneration.Configuration
{
    public static class StringExtensions
    {
        public static bool HasContent(this string? value)
        {
            return !String.IsNullOrEmpty(value);
        }

        public static bool EqualsIgnoringCase(this string value, string other)
        {
            return value.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
