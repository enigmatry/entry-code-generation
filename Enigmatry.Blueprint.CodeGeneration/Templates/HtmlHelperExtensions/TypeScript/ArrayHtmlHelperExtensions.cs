using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Blueprint.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript
{
    public static class ArrayHtmlHelperExtensions
    {
        private static readonly string _elementSeparator = ", ";

        public static IHtmlContent ArrayFor<TSource>(this IHtmlHelper html, IEnumerable<TSource> collection)
        {
            return html.Raw($"[{String.Join(_elementSeparator, collection)}]");
        }

        public static IHtmlContent ArrayFor<TSource, TResult>(this IHtmlHelper html, IEnumerable<TSource> collection, Func<TSource, TResult> selector)
        {
            return ArrayFor(html, collection.Select(selector));
        }

        public static IHtmlContent StringArrayFor<TSource>(this IHtmlHelper html, IEnumerable<TSource> collection)
        {
            return ArrayFor(html, collection.Select(AsString));
        }

        public static IHtmlContent StringArrayFor<TSource, TResult>(this IHtmlHelper html, IEnumerable<TSource> collection, Func<TSource, TResult> selector)
        {
            return ArrayFor(html, collection.Select(selector).Select(AsString));
        }

        private static string AsString<TSource>(TSource source)
        {
            return $"'{source}'";
        }
    }
}
