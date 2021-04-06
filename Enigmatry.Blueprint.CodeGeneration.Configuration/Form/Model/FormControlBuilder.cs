using System;
using System.Reflection;
using Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model.Select;
using Humanizer;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model
{
    public class FormControlBuilder
    {
        private readonly string _property;
        private string _displayName;
        private bool _isVisible;
        private bool _isReadonly;
        private string _placeholder;
        private SelectFormControlBuilder _select = null!;

        public PropertyInfo PropertyInfo { get; }
        public FormControlType FormControlType { get; private set; }

        public SelectFormControlBuilder SelectControlBilder => _select;


        public FormControlBuilder(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            _property = propertyInfo.Name.Camelize();
            _displayName = propertyInfo.Name;
            _placeholder = _displayName;
            _isVisible = true;
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
                    SelectType = _select.Build().SelectType,
                    LookupMedhod = _select.Build().LookupMedhod
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
            if (_select == null)
            {
                _select = new SelectFormControlBuilder(PropertyInfo);
            }
            FormControlType = FormControlType.DropDownList;
            return _select;
        }

        public SelectFormControlBuilder IsAutocompleteControl()
        {
            if (_select == null)
            {
                _select = new SelectFormControlBuilder(PropertyInfo);
            }
            FormControlType = FormControlType.Autocomplete;
            return _select;
        }
    }
}
