﻿using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Configuration.Form;
using Enigmatry.CodeGeneration.Configuration.Form.Model;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Validators;
using Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularFormlyValidationHtmlHelperExtensions
    {
        public static IHtmlContent AddValidationTemplateOptions(this IHtmlHelper html, FormControlModel control)
        {
            var templateOptions = control.ValidationRules
                .SelectMany(x => x.FormlyTemplateOptions)
                .Distinct();
            return html.Raw($"{String.Join(",\r\n", templateOptions)},\r\n");
        }

        public static IHtmlContent AddModelOpetions(this IHtmlHelper html, FormControlModel control) =>
            control.Validator == null
                ? html.Raw("")
                : html.Raw($"modelOptions: {{ updateOn: '{control.Validator.Trigger}' }},\r\n");

        public static IHtmlContent AddAsyncValidators(this IHtmlHelper html, FormControlModel control) =>
            control.Validator == null
                ? html.Raw("")
                : html.Raw($"asyncValidators: {{ validation: [ '{control.Validator.Name.Camelize()}' ] }},\r\n");

        public static IHtmlContent AddCustomValidationMessages(this IHtmlHelper html, FormControlModel form, bool enableI18N)
        {
            var validationMessages = form
                .ValidationRules
                .Where(rule => rule.HasCustomMessage)
                .Select(x => enableI18N
                    ? $"{x.FormlyRuleName.ToLowerInvariant()}: {AngularLocalization.Localize(x.MessageTranslationId, x.FormlyValidationMessage)}"
                    : $"{x.FormlyRuleName.ToLowerInvariant()}: '{x.FormlyValidationMessage}'"
                );
            return html.Raw($"{String.Join(",\r\n", validationMessages)}\r\n");
        }

        public static IHtmlContent AddCommonValidationMessages(this IHtmlHelper html, FeatureModule module, bool enableI18N)
        {
            var messages = new Dictionary<string, string>();
            var formControls = module
                .Components.OfType<FormComponentModel>()
                .SelectMany(form => form.FormControls);

            foreach (var control in formControls)
            {
                foreach (var rule in control.ValidationRules.Where(x => !x.HasCustomMessage))
                {
                    if (!messages.ContainsKey(rule.FormlyRuleName))
                    {
                        var message = enableI18N
                            ? AngularLocalization.Localize(rule.MessageTranslationId, rule.FormlyValidationMessage)
                            : $"`{rule.FormlyValidationMessage}`";
                        messages.Add(
                            rule.FormlyRuleName.ToLowerInvariant(),
                            $"{{ name: '{rule.FormlyRuleName.ToLowerInvariant()}', message: (err, field) => {message} }}"
                        );
                    }
                }
            }

            return html.Raw($"{String.Join(",\r\n", messages.Values)}\r\n");
        }

        public static IHtmlContent ImportValidators(this IHtmlHelper html, FeatureModule module) =>
            module.HasFormValidators
                ? html.ImportStatement("CustomValidatorsService, customValidatorsFactory", "src/app/shared/validators/custom-validators")
                : html.Raw("");

        public static IHtmlContent AddFromValidationProvider(this IHtmlHelper html, FeatureModule module) =>
            module.HasFormValidators
                ? html.Raw(
                    "CustomValidatorsService,\r\n" +
                    "{ provide: FORMLY_CONFIG, multi: true, useFactory: customValidatorsFactory, deps: [ CustomValidatorsService ] }")
                : html.Raw("");
    }
}