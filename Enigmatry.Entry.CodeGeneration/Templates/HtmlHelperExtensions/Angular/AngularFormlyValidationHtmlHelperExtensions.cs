using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularFormlyValidationHtmlHelperExtensions
{
    public static IHtmlContent AddValidationTemplateOptions(this IHtmlHelper html, FormControl control)
    {
        var templateOptions = control.ValidationRules
            .SelectMany(x => x.FormlyTemplateOptions)
            .Distinct();
        return html.Raw($"{String.Join(",\r\n", templateOptions)},\r\n");
    }

    public static IHtmlContent AddAsyncValidators(this IHtmlHelper html, FormControl control) =>
        control.Validators.Any()
            ? html.Raw($"asyncValidators: {{ validation: [ {String.Join(", ", control.Validators.Select(x => $"'{x.Name.Camelize()}'"))} ] }},\r\n")
            : html.Raw("");

    public static IHtmlContent AddCustomValidationMessages(this IHtmlHelper html, FormControl form, bool enableI18N)
    {
        var validationMessages = form
            .ValidationRules
            .Where(rule => rule.HasCustomMessage)
            .Select(x => enableI18N
                ? $"{x.FormlyRuleName}: (err, field) => {AngularLocalization.Localize(x.MessageTranslationId, x.FormlyValidationMessage)}"
                : $"{x.FormlyRuleName}: '{x.FormlyValidationMessage}'"
            );
        return html.Raw($"{String.Join(",\r\n", validationMessages)}\r\n");
    }

    public static IHtmlContent AddCommonValidationMessages(this IHtmlHelper html, FeatureModule module, bool enableI18N)
    {
        var messages = new Dictionary<string, string>();
        var formControls = module
            .Components.OfType<FormComponentModel>()
            .SelectMany(form => form.FormControlsOfType<FormControl>());

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
                        rule.FormlyRuleName,
                        $"{{ name: '{rule.FormlyRuleName}', message: (err, field) => {message} }}"
                    );
                }
            }
        }

        return html.Raw($"{String.Join(",\r\n", messages.Values)}\r\n");
    }

    public static IHtmlContent ImportValidators(this IHtmlHelper html, FeatureModule module, string validatorsPath) =>
        module.HasFormValidators
            ? html.ImportStatement(
                "CustomValidatorsService, customValidatorsFactory", validatorsPath)
            : html.Raw("");

    public static IHtmlContent AddFromValidationProvider(this IHtmlHelper html, FeatureModule module) =>
        module.HasFormValidators
            ? html.Raw(
                "CustomValidatorsService,\r\n" +
                "{ provide: FORMLY_CONFIG, multi: true, useFactory: customValidatorsFactory, deps: [ CustomValidatorsService ] }")
            : html.Raw("");
}
