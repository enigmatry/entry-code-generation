using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Default error markup for validators that have no configured message: named async validators
/// (CustomValidator carries only a name) and Validators.required added at runtime through
/// fieldsRequiredExpressions. Formly surfaced both via its global message registry.
/// </summary>
internal static class AngularSignalsDefaultErrorRenderers
{
    // A named validator that shares its error key with a configured validation rule gets no
    // default error line — the rule already renders the configured message for that key, and two
    // guards on the same key would show both texts at once.
    internal static string AsyncValidatorErrors(this FormControl field, FormViewRenderContext context) =>
        String.Concat(field.Validators
            .Where(validator => field.ValidationRules.All(validationRule => validationRule.AngularErrorKey() != validator.Name.Camelize()))
            .Select(validator =>
            {
                var i18nAttribute = context.EnableI18N ? $" i18n=\"@@validators.{validator.Name.Kebaberize()}\"" : "";
                return
                    $"@if ({context.FormGroupAccessor}.get('{field.PropertyName}')?.hasError('{validator.Name.Camelize()}')) {{\r\n" +
                    $"    <mat-error{i18nAttribute}>{validator.Name.Humanize()} validation failed</mat-error>\r\n" +
                    $"}}";
            }));

    internal static string DynamicRequiredError(this FormControl field, FormViewRenderContext context)
    {
        if (field.ValidationRules.Any(validationRule => validationRule.GetRuleName() == "required"))
        {
            return "";
        }

        var fieldDisplayName = field.Label.Value.HasContent() ? field.Label.Value : field.PropertyName.Humanize();
        var translationId =
            $"{field.ComponentInfo.Feature.Name.Kebaberize()}" +
            $".{field.ComponentInfo.Name.Kebaberize()}" +
            $".{context.TranslationIdSegment(field)}" +
            $".required";
        var i18nAttribute = context.EnableI18N ? $" i18n=\"@@{translationId}\"" : "";

        return
            $"@if ({context.FormGroupAccessor}.get('{field.PropertyName}')?.hasError('required')) {{\r\n" +
            $"    <mat-error{i18nAttribute}>{$"{fieldDisplayName} is required".EscapeHtmlText()}</mat-error>\r\n" +
            $"}}";
    }
}
