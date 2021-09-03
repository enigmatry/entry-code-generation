using Enigmatry.BuildingBlocks.Validation.ValidationRules;
using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public class FormControlModel
    {
        public ComponentInfo ComponentInfo { get; set; } = null!;
        public string PropertyName { get; set; } = String.Empty;
        public string Label { get; set; } = String.Empty;
        public string Placeholder { get; set; } = String.Empty;
        public string Hint { get; set; } = String.Empty;
        public bool IsVisible { get; set; }
        public bool IsReadonly { get; set; }
        public FormControlType Type { get; set; }
        public string LabelTranslationId { get; set; } = String.Empty;
        public string PlaceholderTranslationId { get; set; } = String.Empty;
        public string HintTranslationId { get; set; } = String.Empty;
        public Type? ValueType { get; set; }
        public IList<IValidationRule> ValidationRules { get; set; } = new List<IValidationRule>();


        public string GetFormlyFieldType()
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
    }
}
