using System;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Select;
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
            if (_select != null)
            {
                var select = _select.Build();
                return Build<SelectFormControl>(componentInfo, control =>
                {
                    control.FixedOptions = select.FixedOptions;
                    control.OptionValueKey = select.OptionValueKey;
                    control.OptionDisplayKey = select.OptionDisplayKey;
                });
            }
            return Build<FormControl>(componentInfo);
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
