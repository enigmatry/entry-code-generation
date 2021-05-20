using System;
using System.Diagnostics.CodeAnalysis;

namespace Enigmatry.CodeGeneration.Configuration
{
    public static class StringExtensions
    {
        public static bool HasContent([AllowNull] this string value)
        {
            return !String.IsNullOrEmpty(value);
        }
    }
}
