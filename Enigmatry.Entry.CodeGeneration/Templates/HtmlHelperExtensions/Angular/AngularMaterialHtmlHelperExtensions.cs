using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularMaterialHtmlHelperExtensions
{
    public static IHtmlContent Appearance(this IHtmlHelper html, FormControlAppearance? appearance)
    {
        if (!appearance.HasValue)
        {
            return html.Raw("");
        }

        var appearanceValue = appearance.Value.GetDescription();
        return html.Raw($"appearance: '{appearanceValue}',\r\n");
    }

    public static IHtmlContent FloatLabel(this IHtmlHelper html, FormControlFloatLabel? floatLabel)
    {
        if (!floatLabel.HasValue)
        {
            return html.Raw("");
        }

        var floatLabelValue = floatLabel.Value.GetDescription();
        return html.Raw($"floatLabel: '{floatLabelValue}',\r\n");
    }
}
