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

            foreach (var className in control.ClassNames.Values)
            {
                classNameValue += $" {ApplyOptionally(className)}";
            }

            return html.Raw($"className: `{classNameValue}`,\r\n");
        }

        public static IHtmlContent GroupCssClass(this IHtmlHelper html, FormControlGroup controlGroup)
        {
            string classNameValue = "entry-field-group";

            foreach (var className in controlGroup.ClassNames.Values)
            {
                classNameValue += $" {ApplyOptionally(className)}";
            }

            return html.Raw($"fieldGroupClassName: `{classNameValue}`,\r\n");
        }

        private static string ApplyOptionally(OptionallyAppliedValue<string> className)
        {
            return className.When switch
            {
                ApplyWhen.FormIsReadonly => $"${{this.applyOptionally('{className}', this.isReadonly)}}",
                ApplyWhen.FormIsNotReadonly => $"${{this.applyOptionally('{className}', !this.isReadonly)}}",
                ApplyWhen.Always => $"{className}",
                _ => $"{className}"
            };
        }
    }
}
