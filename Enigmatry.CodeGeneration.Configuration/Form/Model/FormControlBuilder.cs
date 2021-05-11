using System;
using System.Reflection;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Select;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public class FormControlBuilder
    {
        private readonly string _property;
        private string _displayName;
        private bool _isVisible;
        private bool _isReadonly;
        private string _placeholder;

        public PropertyInfo PropertyInfo { get; }
        public FormControlType FormControlType { get; private set; }
        public SelectFormControlBuilder Select { get; private set; } = null!;


        public FormControlBuilder(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            _property = propertyInfo.Name.Camelize();
            _displayName = propertyInfo.Name;
            _placeholder = _displayName;
            _isVisible = !propertyInfo.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
            _isReadonly = false;

            SetDefaultFormControlType();
        }

        private void SetDefaultFormControlType()
        {
            var propertyType = PropertyInfo.PropertyType;
            switch (propertyType)
            {
                case { } when propertyType == typeof(DateTime):
                case { } when propertyType == typeof(DateTimeOffset):
                    FormControlType = FormControlType.DateTime;
                    break;
                case { } when propertyType == typeof(bool):
                    FormControlType = FormControlType.CheckBox;
                    break;
                default:
                    FormControlType = FormControlType.Input;
                    break;
            }
        }

        public FormControlModel Build() =>
            FormControlType switch
            {
                var type when type is FormControlType.Autocomplete || type is FormControlType.DropDownList => new SelectFormControlModel
                {
                    Property = _property,
                    DisplayName = _displayName,
                    IsVisible = _isVisible,
                    IsReadonly = _isReadonly,
                    Placeholder = _placeholder,
                    Type = FormControlType,
                    LookupMedhod = Select.Build().LookupMedhod
                },
                _ => new FormControlModel
                {
                    Property = _property,
                    DisplayName = _displayName,
                    IsVisible = _isVisible,
                    IsReadonly = _isReadonly,
                    Placeholder = _placeholder,
                    Type = FormControlType
                }
            };

        public FormControlBuilder WithLabel(string displayName)
        {
            _displayName = displayName;
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

        public SelectFormControlBuilder IsDropDownListControl()
        {
            if (Select == null)
            {
                Select = new SelectFormControlBuilder(PropertyInfo);
            }
            FormControlType = FormControlType.DropDownList;
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
    }
}
