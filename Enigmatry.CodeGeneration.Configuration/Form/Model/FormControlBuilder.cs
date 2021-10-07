using System;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Select;
using Humanizer;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public class FormControlBuilder : BaseControlBuilder<FormControlBuilder>
    {
        private SelectFormControlBuilder? _select;

        public FormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            _formControlType = GetDefaultFormControlType(propertyInfo);
        }

        public FormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var translationId = $"{componentInfo.TranslationId}.{_propertyName.Kebaberize()}.";
            var labelTranslationId = _labelTranslationId ?? $"{translationId}label";
            var placeholderTranslationId = _placeholderTranslationId ?? $"{translationId}placeholder";
            var hintTranslationId = _hintTranslationId ?? $"{translationId}hint";

            if (_formControlType == FormControlType.Select ||
                _formControlType == FormControlType.MultiSelect ||
                _formControlType == FormControlType.Autocomplete ||
                _formControlType == FormControlType.Radio)
            {
                return new SelectFormControl
                {
                    ComponentInfo = componentInfo,
                    PropertyName = _propertyName,
                    Label = _label.Humanize(),
                    Placeholder = _placeholder.Humanize(),
                    Hint = _hint,
                    IsVisible = _isVisible,
                    IsReadonly = _isReadonly,
                    Type = _formControlType,
                    LookupMethod = _select?.Build().LookupMethod!,
                    LabelTranslationId = labelTranslationId,
                    PlaceholderTranslationId = placeholderTranslationId,
                    HintTranslationId = hintTranslationId,
                    Validator = _validator,
                    ClassName = _className
                };
            }
            return new FormControl
            {
                ComponentInfo = componentInfo,
                PropertyName = _propertyName,
                Label = _label.Humanize(),
                Placeholder = _placeholder.Humanize(),
                Hint = _hint,
                IsVisible = _isVisible,
                IsReadonly = _isReadonly,
                Type = _formControlType,
                ValueType = PropertyInfo?.PropertyType,
                LabelTranslationId = labelTranslationId,
                PlaceholderTranslationId = placeholderTranslationId,
                HintTranslationId = hintTranslationId,
                Validator = _validator,
                ClassName = _className
            };
        }

        public SelectFormControlBuilder IsDropDownListControl()
        {
            _select ??= new SelectFormControlBuilder(_propertyName);
            _formControlType = FormControlType.Select;
            return _select;
        }

        public SelectFormControlBuilder IsAutocompleteControl()
        {
            _select ??= new SelectFormControlBuilder(_propertyName);
            _formControlType = FormControlType.Autocomplete;
            return _select;
        }

        public SelectFormControlBuilder IsRadioGroupControl()
        {
            _select ??= new SelectFormControlBuilder(_propertyName);
            _formControlType = FormControlType.Radio;
            return _select;
        }

        private static FormControlType GetDefaultFormControlType(PropertyInfo propertyInfo)
        {
            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            switch (propertyType)
            {
                case { } when propertyType == typeof(DateTime):
                case { } when propertyType == typeof(DateTimeOffset):
                    return FormControlType.Datepicker;
                case { } when propertyType == typeof(bool):
                    return FormControlType.CheckBox;
                default:
                    return FormControlType.Input;
            }
        }
    }
}
