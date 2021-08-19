using Enigmatry.CodeGeneration.Configuration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularLocalization
    {
        public static IHtmlContent Localize(this IHtmlHelper html, string id, string text, bool enableI18N)
        {
            return enableI18N && text.HasContent() ? html.Raw(Localize(id, text)) : html.Raw($"'{text}'");
        }

        public static string Localize(string id, string text)
        {
            return $"$localize `:@@{id}:{text}`";
        }
    }
}
