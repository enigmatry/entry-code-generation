using System;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Select;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public class FormControlBuilder : BaseControlBuilder<FormControlBuilder>
    {
        private SelectControlOptionsBuilder? _selectOptionsBuilder;

        public FormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            _formControlType = GetDefaultFormControlType(propertyInfo);
        }

        public FormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            if (_selectOptionsBuilder != null)
            {
                return Build<SelectFormControl>(componentInfo, control => control.Options = _selectOptionsBuilder.Build());
            }

            return Build<FormControl>(componentInfo);
        }

        public FormControlBuilder IsDropDownListControl(Action<SelectControlOptionsBuilder>? options = null)
        {
            _formControlType = FormControlType.Select;
            _selectOptionsBuilder = new SelectControlOptionsBuilder();
            options?.Invoke(_selectOptionsBuilder);
            return this;
        }

        public FormControlBuilder IsAutocompleteControl(Action<SelectControlOptionsBuilder>? options = null)
        {
            _formControlType = FormControlType.Autocomplete;
            _selectOptionsBuilder = new SelectControlOptionsBuilder();
            options?.Invoke(_selectOptionsBuilder);
            return this;
        }

        public FormControlBuilder IsRadioGroupControl(Action<SelectControlOptionsBuilder>? options = null)
        {
            _formControlType = FormControlType.Radio;
            _selectOptionsBuilder = new SelectControlOptionsBuilder();
            options?.Invoke(_selectOptionsBuilder);
            return this;
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
