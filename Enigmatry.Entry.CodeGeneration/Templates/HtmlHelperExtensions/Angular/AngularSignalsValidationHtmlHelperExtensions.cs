using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsValidationHtmlHelperExtensions
{
    public static string AngularValidators(this IHtmlHelper htmlHelper, FormControl control)
    {
        var validators = new List<string>();
        foreach (var rule in control.ValidationRules)
        {
            if (rule.RuleName == "required")
            {
                validators.Add("Validators.required");
            }
            else
            {
                var valueOption = rule.TemplateOptions
                    .FirstOrDefault(templateOption => templateOption.StartsWith($"{rule.RuleName}: ", StringComparison.Ordinal));
                if (valueOption != null)
                {
                    var value = valueOption[(rule.RuleName.Length + 2)..];
                    validators.Add($"Validators.{rule.RuleName}({value})");
                }
            }
        }

        return validators.Count > 0 ? $"[{String.Join(", ", validators)}]" : "[]";
    }
}
