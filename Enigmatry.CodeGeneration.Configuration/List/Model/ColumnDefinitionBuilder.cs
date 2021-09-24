using System;
using System.Reflection;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.Formatters;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.List.Model
{
    public class ColumnDefinitionBuilder
    {
        private readonly PropertyAccessor? _propertyAccessor;
        private readonly string _propertyName;
        private string _headerName;
        private string? _translationId;
        private bool _isVisible;
        private bool _isSortable;
        private IPropertyFormatter _formatter;
        private string? _customCellComponent;
        private string? _customCellCssClass;

        public ColumnDefinitionBuilder(PropertyInfo propertyInfo)
        {
            _propertyAccessor = new PropertyAccessor(propertyInfo);
            _propertyName = _propertyAccessor.Name.Camelize();
            _headerName = _propertyAccessor.DisplayName;
            _isVisible = !_propertyAccessor.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
            _isSortable = true;
            _formatter = _propertyAccessor.GetDefaultPropertyFormatter();
        }

        public ColumnDefinitionBuilder(string propertyName)
        {
            _propertyName = propertyName.Camelize();
            _headerName = propertyName.Humanize();
            _isVisible = true;
            _isSortable = true;
            _formatter = new NoFormattingPropertyFormatter();
        }

        public ColumnDefinition Build(ComponentInfo componentInfo)
        {
            var translationId = _translationId ?? $"{componentInfo.TranslationId}.{_propertyName.Kebaberize()}";

            return new ColumnDefinition
            {
                ComponentInfo = componentInfo,
                Property = _propertyName,
                HeaderName = _headerName,
                TranslationId = translationId,
                IsSortable = _isSortable,
                IsVisible = _isVisible,
                Formatter = _formatter,
                CustomCellComponent = _customCellComponent,
                CustomCellCssClass = _customCellCssClass
            };
        }

        public ColumnDefinitionBuilder WithHeaderName(string headerName)
        {
            _headerName = headerName;
            return this;
        }

        public ColumnDefinitionBuilder WithTranslationId(string translationId)
        {
            _translationId = translationId;
            return this;
        }

        public ColumnDefinitionBuilder IsSortable(bool isSortable)
        {
            _isSortable = isSortable;
            return this;
        }

        public ColumnDefinitionBuilder IsVisible(bool isVisible)
        {
            _isVisible = isVisible;
            return this;
        }

        public ColumnDefinitionBuilder WithFormat(IPropertyFormatter formatter)
        {
            if (_propertyAccessor != null)
            {
                formatter.ValidateInputType(_propertyAccessor.PropertyType);
            }
            _formatter = formatter;
            return this;
        }

        public ColumnDefinitionBuilder WithCustomComponent(string componentName)
        {
            _customCellComponent = componentName;
            return this;
        }

        public ColumnDefinitionBuilder WithCustomCssClass(string cssClassName)
        {
            _customCellCssClass = cssClassName;
            return this;
        }

        public bool HasProperty(PropertyInfo propertyInfo)
        {
            return _propertyAccessor != null && _propertyAccessor.PropertyInfo == propertyInfo;
        }

        public bool HasProperty(string propertyName)
        {
            return _propertyName == propertyName;
        }
    }
}
