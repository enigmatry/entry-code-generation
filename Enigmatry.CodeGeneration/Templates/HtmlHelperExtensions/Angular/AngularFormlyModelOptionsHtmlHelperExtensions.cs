using Enigmatry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularFormlyModelOptionsHtmlHelperExtensions
    {
        public static IHtmlContent AddModelOptions(this IHtmlHelper html, FormControl control)
        {
            var eventName = control.ValueUpdateTrigger switch
            {
                ValueUpdateTrigger.OnBlur => "blur",
                ValueUpdateTrigger.OnChange => "change",
                ValueUpdateTrigger.OnSubmit => "submit",
                _ => null
            };
            return eventName == null ? html.Raw("") : html.Raw($"modelOptions: {{ updateOn: '{eventName}' }},\r\n");
        }
    }
}
