using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Default error markup for validators that have no configured message: named async validators
/// (CustomValidator carries only a name) and Validators.required added at runtime through
/// fieldsRequiredExpressions. Formly surfaced both via its global message registry.
/// </summary>
internal static class AngularSignalsDefaultErrorRenderers
{
    internal static string AsyncValidatorErrors(this FormControl field, FormViewRenderContext context) =>
        String.Concat(field.Validators.Select(validator =>
        {
            var i18nAttribute = context.EnableI18N ? $" i18n=\"@@validators.{validator.Name.Kebaberize()}\"" : "";
            return
                $"@if ({context.FormGroupAccessor}.get('{field.PropertyName}')?.hasError('{validator.Name.Camelize()}')) {{\r\n" +
                $"    <mat-error{i18nAttribute}>{validator.Name.Humanize()} validation failed</mat-error>\r\n" +
                $"}}";
        }));

    internal static string DynamicRequiredError(this FormControl field, FormViewRenderContext context)
    {
        if (field.ValidationRules.Any(validationRule => validationRule.RuleName == "required"))
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
