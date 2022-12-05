using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularFormlyHtmlHelperExtensions
    {
        public static IHtmlContent FieldCssClass(this IHtmlHelper html, FormControl control)
        {
            string classNameValue = $"entry-{control.PropertyName.Kebaberize()}-field entry-{control.FormlyType.Kebaberize()}";
            if (control.ClassName.HasContent())
            {
                classNameValue += $" {control.ClassName}";
            }
            return html.Raw($"className: '{classNameValue}',\r\n");
        }

        public static IHtmlContent GroupCssClass(this IHtmlHelper html, FormControlGroup controlGroup)
        {
            string classNameValue = $"entry-field-group";
            if (controlGroup.ClassName.HasContent())
            {
                classNameValue += $" {controlGroup.ClassName}";
            }
            return html.Raw($"fieldGroupClassName: '{classNameValue}',\r\n");
        }
    }
}
