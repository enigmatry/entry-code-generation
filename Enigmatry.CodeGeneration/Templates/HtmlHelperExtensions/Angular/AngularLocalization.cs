using Enigmatry.CodeGeneration.Configuration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularLocalization
    {
        public static IHtmlContent Localize(this IHtmlHelper html, string key, string text, bool enableI18N)
        {
            return enableI18N && text.HasContent() ? html.Raw(Localize(key, text)) : html.Raw($"'{text}'");
        }

        public static IHtmlContent Localize(this IHtmlHelper html, I18NString text, bool enableI18N)
        {
            return enableI18N && text.HasContent() ? html.Raw(Localize(text.Key, text.Value)) : html.Raw($"'{text.Value}'");
        }

        public static string Localize(string key, string value)
        {
            return $"$localize `:@@{key}:{value}`";
        }
    }
}
