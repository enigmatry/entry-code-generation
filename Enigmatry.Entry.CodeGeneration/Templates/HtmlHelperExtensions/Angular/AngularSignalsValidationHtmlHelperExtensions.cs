using System.Globalization;
using System.Text.RegularExpressions;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;
using Humanizer;
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

    internal static string RenderValidationErrors(this IHtmlHelper htmlHelper, FormControl field, FormViewRenderContext context) =>
        String.Concat(field.ValidationRules.Select(validationRule =>
        {
            var rawMessage = validationRule.HasCustomMessage ? validationRule.CustomMessage : validationRule.ValidationMessage;
            var message = field.ResolvedValidationMessage(validationRule);

            // A message that got field-specific values interpolated into it can no longer share
            // a translation id with other fields, so it gets the same per-field id shape that
            // FormControl.ApplyValidationConfiguration mints.
            var translationId = message == rawMessage
                ? validationRule.MessageTranslationId
                : $"{field.ComponentInfo.Feature.Name.Kebaberize()}" +
                  $".{field.ComponentInfo.Name.Kebaberize()}" +
                  $".{field.PropertyName.Kebaberize()}" +
                  $".{validationRule.RuleName.Kebaberize()}";
            var i18nAttribute = context.EnableI18N && translationId.HasContent() ? $" i18n=\"@@{translationId}\"" : "";

            return
                $"@if ({context.FormGroupAccessor}.get('{field.PropertyName}')?.hasError('{validationRule.AngularErrorKey()}')) {{\r\n" +
                $"    <mat-error{i18nAttribute}>{message}</mat-error>\r\n" +
                $"}}";
        }));

    // Angular's built-in Validators.minLength/maxLength report their errors under
    // all-lowercase keys, unlike the camelCase rule names used for the validator factories.
    private static string AngularErrorKey(this IFormlyValidationRule validationRule) => validationRule.RuleName switch
    {
        "minLength" => "minlength",
        "maxLength" => "maxlength",
        _ => validationRule.RuleName
    };

    // Default rule messages carry Formly-era runtime interpolations such as
    // "${field?.templateOptions?.label}:property-name:"; the label and the rule
    // values are known at generation time, so they are resolved into plain text here.
    private static string ResolvedValidationMessage(this FormControl field, IFormlyValidationRule validationRule)
    {
        var message = validationRule.HasCustomMessage ? validationRule.CustomMessage : validationRule.ValidationMessage;

        return Regex.Replace(message, @"\$\{field\?\.templateOptions\?\.(\w+)\}:[\w-]+:", match =>
        {
            var propertyReference = match.Groups[1].Value;
            if (propertyReference == "label")
            {
                return field.Label.Value;
            }

            var valueText = validationRule.TemplateOptions
                .FirstOrDefault(templateOption => templateOption.StartsWith($"{propertyReference}: ", StringComparison.Ordinal))
                ?[(propertyReference.Length + 2)..];

            return valueText == null ? match.Value : EvaluateNumericExpression(valueText);
        });
    }

    private static string EvaluateNumericExpression(string expression)
    {
        var parts = expression.Split(new[] { '+', '-' }, StringSplitOptions.TrimEntries);
        if (parts.Length == 2
            && decimal.TryParse(parts[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var left)
            && decimal.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var right))
        {
            var result = expression.Contains('+') ? left + right : left - right;
            return result.ToString(CultureInfo.InvariantCulture);
        }

        return expression;
    }
}
