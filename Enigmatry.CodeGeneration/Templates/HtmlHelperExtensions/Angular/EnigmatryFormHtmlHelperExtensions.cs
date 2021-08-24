using Enigmatry.BuildingBlocks.Validation.ValidationRules;
using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Configuration.Form.Model;
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

            if (Check.IsNumber(control.ValueType))
            {
                htmlContent.Add($"type: 'number'");
            };

            foreach (var validationRule in control.ValidationRules)
            {
                htmlContent.Add(validationRule.AsNameValueString());
            }

            return html.Raw($"{String.Join(",\n", htmlContent)},\n");
        }

        public static IHtmlContent AddValidationMessages(this IHtmlHelper html, FormControlModel form, bool enableI18N)
        {
            var validationMessage = form
                .ValidationRules
                .Select(x => enableI18N
                    ? $"{x.Name}: {AngularLocalization.Localize(x.MessageTranslationId, x.Message)}"
                    : $"{x.Name}: '{x.Message}'");
            return html.Raw($"{String.Join(",\n", validationMessage)}\n");
        }
    }
}
