using Enigmatry.BuildingBlocks.Validation.ValidationRules.CustomValidationRules;
using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Configuration.Form.Model;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class EnigmatryFormHtmlHelperExtensions
    {
        public static IHtmlContent AddValidationFields(this IHtmlHelper html, FormControlModel control)
        {
            var htmlContent = new List<string>();

            if (control.ValueType != null && Check.IsNumber(control.ValueType))
            {
                htmlContent.Add($"type: 'number'");
            }

            foreach (var validationRule in control.BuiltInValidationRules)
            {
                htmlContent.Add(validationRule.AsNameRulePair());
            }

            return html.Raw($"{String.Join(",\n", htmlContent)},\n");
        }

        public static IHtmlContent AddValidationMessages(this IHtmlHelper html, FormControlModel form, bool enableI18N)
        {
            var validationMessage = form
                .BuiltInValidationRules
                .Select(x => enableI18N
                    ? $"{x.Name}: {AngularLocalization.Localize(x.MessageTranslationId, x.Message)}"
                    : $"{x.Name}: '{x.Message}'");
            return html.Raw($"{String.Join(",\n", validationMessage)}\n");
        }

        public static IHtmlContent ImportValidators(this IHtmlHelper html, IEnumerable<FormControlModel> formControls, string validatorsPath)
        {
            var validatorNames = formControls
                .SelectMany(control => new[] { control.CustomValidator?.ValidatorName ?? "", control.AsyncCustomValidator?.ValidatorName ?? "" })
                .Where(validatorName => !String.IsNullOrEmpty(validatorName))
                .Distinct();
            return html.Raw($"import {{ {String.Join(", ", validatorNames.Distinct())} }} from '{validatorsPath}';");
        }

        public static IHtmlContent AddValidators(this IHtmlHelper html, string propertyName, CustomValidatorValidationRule validator, bool enableI18N) =>
            html.Raw($"validators: {{ {propertyName.Camelize()}: {{ " +
                $"expression: {validator.ValidatorName}, " +
                $"message: {(enableI18N ? AngularLocalization.Localize(validator.MessageTranslationId, validator.Message) : $"'{validator.Message}'")} }} }},");

        public static IHtmlContent AddAsuncValidators(this IHtmlHelper html, string propertyName, AsyncCustomValidatorValidationRule validator, bool enableI18N) =>
            html.Raw($"asyncValidators: {{ {propertyName.Camelize()}: {{ " +
                $"expression: {validator.ValidatorName}, " +
                $"message: {(enableI18N ? AngularLocalization.Localize(validator.MessageTranslationId, validator.Message) : $"'{validator.Message}'")} }} }},");
    }
}
