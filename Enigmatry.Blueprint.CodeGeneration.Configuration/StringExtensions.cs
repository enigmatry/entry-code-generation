using System;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    internal static class StringExtensions
    {
        public static bool HasContent(this string value)
        {
            return !String.IsNullOrEmpty(value);
        }
    }
}
