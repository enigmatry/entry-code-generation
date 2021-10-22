using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.BuildingBlocks.Validation.ValidationRules;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Validators;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public class FormControl
    {
        public ComponentInfo ComponentInfo { get; set; } = null!;
        public string PropertyName { get; set; } = String.Empty;
        public string Label { get; set; } = String.Empty;
        public string Placeholder { get; set; } = String.Empty;
        public string Hint { get; set; } = String.Empty;
        public bool IsVisible { get; set; }
        public bool IsReadonly { get; set; }
        public string? ClassName { get; set; } = String.Empty;
        public string LabelTranslationId { get; set; } = String.Empty;
        public string PlaceholderTranslationId { get; set; } = String.Empty;
        public string HintTranslationId { get; set; } = String.Empty;
        public FormControlType Type { get; set; }
        public Type? ValueType { get; set; }
        public IList<IFormlyValidationRule> ValidationRules { get; private set; } = new List<IFormlyValidationRule>();
        public CustomValidator? Validator { get; set; }

        public virtual string GetFormlyType()
        {
            switch (Type)
            {
                case FormControlType.Input:
                    return "input";
                case FormControlType.Textarea:
                    return "textarea";
                case FormControlType.CheckBox:
                    return "checkbox";
                case FormControlType.Select:
                case FormControlType.MultiSelect:
                    return "select";
                case FormControlType.Datepicker:
                    return "datepicker";
                case FormControlType.Autocomplete:
                    return "autocomplete";
                case FormControlType.Radio:
                    return "radio";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string GetReadonlyFormlyType()
        {
            return Type switch
            {
                FormControlType.CheckBox => "readonly-boolean",
                FormControlType.Radio => "readonly-radio",
                _ => GetFormlyType(),
            };
        }

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
    }
}
