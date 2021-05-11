using System;
using System.Reflection;
using Enigmatry.CodeGeneration.Configuration.Formatters;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.List.Model
{
    public class ColumnPropertyBuilder
    {
        public PropertyInfo PropertyInfo { get; }
        private readonly string _property;
        private string _displayName;
        private string? _translationId;
        private bool _isVisible;
        private bool _isSortable;
        private IPropertyFromatter _formatter;
        private string? _customComponentName;

        public ColumnPropertyBuilder(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            _property = propertyInfo.Name.Camelize();
            _displayName = propertyInfo.Name;
            _isVisible = !propertyInfo.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
            _isSortable = true;
            _formatter = GetDefaultPropertyFormatter(PropertyInfo);
        }

        public ColumnPropertyModel Build()
        {
            return new ColumnPropertyModel
            {
                Property = _property,
                DisplayName = _displayName,
                TranslationId = _translationId,
                IsSortable = _isSortable,
                IsVisible = _isVisible,
                Formatter = _formatter,
                CustomComponentName = _customComponentName
            };
        }

        public ColumnPropertyBuilder WithDisplayName(string displayName)
        {
            _displayName = displayName;
            return this;
        }

        public ColumnPropertyBuilder WithTranslationId(string translationId)
        {
            _translationId = translationId;
            return this;
        }

        public ColumnPropertyBuilder IsSortable(bool isSortable)
        {
            _isSortable = isSortable;
            return this;
        }

        public ColumnPropertyBuilder IsVisible(bool isVisible)
        {
            _isVisible = isVisible;
            return this;
        }

        public ColumnPropertyBuilder WithFormat(IPropertyFromatter formatter)
        {
            if (formatter.ValidateInputType(PropertyInfo.PropertyType))
            {
                _formatter = formatter;
            }
            return this;
        }

        public ColumnPropertyBuilder WithCustomComponent(string componentName)
        {
            _customComponentName = componentName;
            return this;
        }


        private static IPropertyFromatter GetDefaultPropertyFormatter(PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;
            switch (propertyType)
            {
                case { } when propertyType == typeof(DateTime):
                case { } when propertyType == typeof(DateTimeOffset):
                    return new DatePropertyFormatter();
                case { } when propertyType == typeof(float):
                case { } when propertyType == typeof(double):
                case { } when propertyType == typeof(decimal):
                    return new DecimalPropertyFormatter();
                default:
                    return new NoFormattingPropertyFormatter();
            }
        }
    }
}
