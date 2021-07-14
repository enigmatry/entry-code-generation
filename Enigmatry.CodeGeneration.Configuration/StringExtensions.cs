using System;

namespace Enigmatry.CodeGeneration.Configuration
{
    public static class StringExtensions
    {
        public static bool HasContent(this string? value)
        {
            return !String.IsNullOrEmpty(value);
        }
    }
}
