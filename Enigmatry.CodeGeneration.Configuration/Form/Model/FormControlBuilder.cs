using System;
using System.Reflection;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Select;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public class FormControlBuilder
    {
        private readonly string _propertyName;
        private string _label;
        private bool _isVisible;
        private bool _isReadonly;
        private string _placeholder;
        private string _hint;
        private string? _labelTranslationId;
        private string? _placeholderTranslationId;
        private string? _hintTranslationId;

        public PropertyInfo PropertyInfo { get; }
        public FormControlType FormControlType { get; private set; }
        public SelectFormControlBuilder Select { get; private set; } = null!;


        public FormControlBuilder(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            _propertyName = propertyInfo.Name.Camelize();
            _label = propertyInfo.Name;
            _placeholder = _label;
            _isVisible = !propertyInfo.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
            _isReadonly = false;
            _hint = String.Empty;

            SetDefaultFormControlType();
        }

        private void SetDefaultFormControlType()
        {
            var propertyType = PropertyInfo.PropertyType;
            switch (propertyType)
            {
                case { } when propertyType == typeof(DateTime):
                case { } when propertyType == typeof(DateTimeOffset):
                    FormControlType = FormControlType.Datepicker;
                    break;
                case { } when propertyType == typeof(bool):
                    FormControlType = FormControlType.CheckBox;
                    break;
                default:
                    FormControlType = FormControlType.Input;
                    break;
            }
        }

        public FormControlModel Build(ComponentInfo componentInfo)
        {
            var translationId = $"{componentInfo.TranslationId}.{_propertyName.Kebaberize()}.";
            var labelTranslationId = _labelTranslationId ?? $"{translationId}label";
            var placeholderTranslationId = _placeholderTranslationId ?? $"{translationId}placeholder";
            var hintTranslationId = _hintTranslationId ?? $"{translationId}hint";

            switch (FormControlType)
            {
                case FormControlType.Select:
                case FormControlType.MultiSelect:
                case FormControlType.Autocomplete:
                case FormControlType.Radio:
                    return new SelectFormControlModel
                    {
                        ComponentInfo = componentInfo,
                        PropertyName = _propertyName,
                        Label = _label.Humanize(),
                        Placeholder = _placeholder.Humanize(),
                        Hint = _hint,
                        IsVisible = _isVisible,
                        IsReadonly = _isReadonly,
                        Type = FormControlType,
                        LookupMethod = Select?.Build().LookupMethod!,
                        LabelTranslationId = labelTranslationId,
                        PlaceholderTranslationId = placeholderTranslationId,
                        HintTranslationId = hintTranslationId
                    };
                default:
                    return new FormControlModel
                    {
                        ComponentInfo = componentInfo,
                        PropertyName = _propertyName,
                        Label = _label.Humanize(),
                        Placeholder = _placeholder.Humanize(),
                        Hint = _hint,
                        IsVisible = _isVisible,
                        IsReadonly = _isReadonly,
                        Type = FormControlType,
                        ValueType = PropertyInfo.PropertyType,
                        LabelTranslationId = labelTranslationId,
                        PlaceholderTranslationId = placeholderTranslationId,
                        HintTranslationId = hintTranslationId
                    };
            }
        }

        public FormControlBuilder WithLabel(string label)
        {
            _label = label;
            return this;
        }

        public FormControlBuilder IsVisible(bool isVisible)
        {
            _isVisible = isVisible;
            return this;
        }

        public FormControlBuilder IsReadonly(bool isReadonly)
        {
            _isReadonly = isReadonly;
            return this;
        }

        public FormControlBuilder WithPlaceholder(string placeholder)
        {
            _placeholder = placeholder;
            return this;
        }

        public FormControlBuilder WithHint(string hint)
        {
            _hint = hint;
            return this;
        }

        public FormControlBuilder WithLabelTranslationId(string translationId)
        {
            _labelTranslationId = translationId;
            return this;
        }

        public FormControlBuilder WithPlaceholderTranslationId(string translationId)
        {
            _placeholderTranslationId = translationId;
            return this;
        }

        public FormControlBuilder WithHintTranslationId(string translationId)
        {
            _hintTranslationId = translationId;
            return this;
        }

        public SelectFormControlBuilder IsDropDownListControl()
        {
            if (Select == null)
            {
                Select = new SelectFormControlBuilder(PropertyInfo);
            }
            FormControlType = FormControlType.Select;
            return Select;
        }

        public SelectFormControlBuilder IsAutocompleteControl()
        {
            if (Select == null)
            {
                Select = new SelectFormControlBuilder(PropertyInfo);
            }
            FormControlType = FormControlType.Autocomplete;
            return Select;
        }

        public SelectFormControlBuilder IsRadioGroupControl()
        {
            if (Select == null)
            {
                Select = new SelectFormControlBuilder(PropertyInfo);
            }
            FormControlType = FormControlType.Radio;
            return Select;
        }
    }
}
