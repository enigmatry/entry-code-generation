using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;

public static class ArrayHtmlHelperExtensions
{
    private static readonly string _elementSeparator = ", ";

    public static IHtmlContent JsArray<TSource>(this IHtmlHelper html, IEnumerable<TSource> collection)
    {
        return html.Raw($"[{String.Join(_elementSeparator, collection)}]");
    }

    public static IHtmlContent JsArray<TSource, TResult>(this IHtmlHelper html, IEnumerable<TSource> collection, Func<TSource, TResult> selector)
    {
        return JsArray(html, collection.Select(selector));
    }

    public static IHtmlContent JsStringArray<TSource>(this IHtmlHelper html, IEnumerable<TSource> collection)
    {
        return JsArray(html, collection.Select(AsString));
    }

    public static IHtmlContent JsStringArray<TSource, TResult>(this IHtmlHelper html, IEnumerable<TSource> collection, Func<TSource, TResult> selector)
    {
        return JsArray(html, collection.Select(selector).Select(AsString));
    }

    private static string AsString<TSource>(TSource source) => $"'{source}'";
}