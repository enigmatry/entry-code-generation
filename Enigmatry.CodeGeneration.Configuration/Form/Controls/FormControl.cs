using Enigmatry.CodeGeneration.Configuration.Form.Controls.Validators;
using Enigmatry.CodeGeneration.Configuration.Formatters;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Validation.ValidationRules;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public abstract class FormControl
    {
        public ComponentInfo ComponentInfo { get; set; } = null!;
        public string PropertyName { get; set; } = String.Empty;
        public I18NString Label { get; set; } = I18NString.Empty;
        public I18NString Placeholder { get; set; } = I18NString.Empty;
        public I18NString Hint { get; set; } = I18NString.Empty;
        public I18NString Tooltip { get; set; } = I18NString.Empty;
        public bool Visible { get; set; }
        public bool Readonly { get; set; }
        public string? ClassName { get; set; } = String.Empty;
        public virtual string? Type { get; set; }
        public FormControlAppearance? Appearance { get; set; }
        public FormControlFloatLabel? FloatLabel { get; set; }
        public IList<IFormlyValidationRule> ValidationRules { get; private set; } = new List<IFormlyValidationRule>();
        public IEnumerable<CustomValidator> Validators { get; set; } = new List<CustomValidator>();
        public FormControlWrappers Wrappers { get; set; } = FormControlWrappers.Default;
        public abstract string FormlyType { get; }
        public IPropertyFormatter? Formatter { get; set; }
        public bool Ignore { get; set; }
        public string? DefaultValue { get; set; }
        public ValueUpdateTrigger? ValueUpdateTrigger { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Metadata { get; set; } = new List<KeyValuePair<string, string>>();

        public virtual void ApplyValidationConfiguration(IEnumerable<IFormlyValidationRule> validationRules)
        {
            ValidationRules = validationRules
                .Where(x => x.PropertyName == PropertyName)
                .ToList();

            var validationRulesWithoutTranslationId = ValidationRules.Where(x => !x.HasMessageTranslationId);
            foreach (var validationRule in validationRulesWithoutTranslationId)
                validationRule.SetMessageTranslationId(
                    $"{ComponentInfo.Feature.Name.Kebaberize()}" +
                    $".{ComponentInfo.Name.Kebaberize()}" +
                    $".{PropertyName.Kebaberize()}" +
                    $".{validationRule.FormlyRuleName.Kebaberize()}"
                );
        }

        public bool IsRequired => ValidationRules.HasRule<IsRequiredValidationRule>();
    }
}
