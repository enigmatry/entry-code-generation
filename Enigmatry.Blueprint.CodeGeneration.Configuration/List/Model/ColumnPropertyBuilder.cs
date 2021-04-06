﻿using Enigmatry.Blueprint.CodeGeneration.Configuration.Formatters;
using Humanizer;
using System;
using System.Reflection;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.List.Model
{
    public class ColumnPropertyBuilder
    {
        public PropertyInfo PropertyInfo { get; }
        private readonly string _property;
        private string _displayName;
        private bool _isVisible;
        private bool _isSortable;
        private IPropertyFromatter _formatter = new NoFormattingPropertyFormatter();

        public ColumnPropertyBuilder(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            _property = propertyInfo.Name.Camelize();
            _displayName = propertyInfo.Name;
            _isVisible = true;
            _isSortable = true;
            _formatter = GetDefaultPropertyFormatter(PropertyInfo);
        }

        public ColumnPropertyModel Build()
        {
            return new ColumnPropertyModel
            {
                Property = _property,
                DisplayName = _displayName,
                IsSortable = _isSortable,
                IsVisible = _isVisible,
                Formatter = _formatter
            };
        }

        public ColumnPropertyBuilder WithDisplayName(string displayName)
        {
            _displayName = displayName;
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
