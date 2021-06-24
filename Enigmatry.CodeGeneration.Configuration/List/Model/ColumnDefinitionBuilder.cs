using System;
using System.Linq.Expressions;
using System.Reflection;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.Formatters;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.List.Model
{
    public class ColumnDefinitionBuilder : IBuilder<ColumnDefinition>
    {
        private readonly PropertyAccessor? _propertyAccessor;
        private readonly string _propertyName;
        private string _headerName;
        private string? _translationId;
        private bool _isVisible;
        private bool _isSortable;
        private IPropertyFormatter _formatter;
        private string? _customCellComponent;

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

        public ColumnDefinition Build()
        {
            return new ColumnDefinition
            {
                Property = _propertyName,
                HeaderName = _headerName,
                TranslationId = _translationId,
                IsSortable = _isSortable,
                IsVisible = _isVisible,
                Formatter = _formatter,
                CustomCellComponent = _customCellComponent
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

        private bool HasProperty(PropertyInfo propertyInfo)
        {
            return _propertyAccessor != null && _propertyAccessor.PropertyInfo == propertyInfo;
        }

        public bool HasProperty(LambdaExpression propertyAccessExpression)
        {
            return HasProperty(propertyAccessExpression.GetPropertyInfo());
        }

        public bool HasProperty(string propertyName)
        {
            return _propertyName == propertyName;
        }
    }
}
