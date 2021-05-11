using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string? value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
        {
            return collection == null || !collection.Any();
        }
    }
}
