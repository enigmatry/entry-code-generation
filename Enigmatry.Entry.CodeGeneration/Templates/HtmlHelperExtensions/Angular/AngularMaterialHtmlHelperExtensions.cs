using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularMaterialHtmlHelperExtensions
    {
        public static IHtmlContent Appearance(this IHtmlHelper html, FormControlAppearance? appearance)
        {
            if (!appearance.HasValue)
            {
                return html.Raw("");
            }

            var appearanceValue = EnumExtensions.GetDescription(appearance.Value);
            return html.Raw($"appearance: '{appearanceValue}',\r\n");
        }

        public static IHtmlContent FloatLabel(this IHtmlHelper html, FormControlFloatLabel? appearance)
        {
            if (!appearance.HasValue)
            {
                return html.Raw("");
            }

            var floatLabelValue = EnumExtensions.GetDescription(appearance.Value);
            return html.Raw($"floatLabel: '{floatLabelValue}',\r\n");
        }
    }
}
