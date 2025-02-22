﻿using System.Linq.Expressions;
using System.Reflection;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Builder;

public class PropertyAccessor
{
    public PropertyInfo PropertyInfo { get; }
    public string Name => PropertyInfo.Name;
    public string DisplayName => PropertyInfo.Name.Humanize();
    public Type PropertyType => Nullable.GetUnderlyingType(PropertyInfo.PropertyType) ?? PropertyInfo.PropertyType;

    public PropertyAccessor(PropertyInfo propertyInfo)
    {
        Check.NotNull(propertyInfo, nameof(propertyInfo));
        PropertyInfo = propertyInfo;
    }

    public PropertyAccessor(LambdaExpression propertyAccessExpression)
        : this(propertyAccessExpression.GetPropertyInfo())
    {
    }

    public IPropertyFormatter GetDefaultPropertyFormatter()
    {
        var propertyType = PropertyType;
        switch (propertyType)
        {
            case { } when propertyType == typeof(DateTime):
            case { } when propertyType == typeof(DateTimeOffset):
#if NET8_0_OR_GREATER
            case { } when propertyType == typeof(DateOnly):
#endif
                return new DatePropertyFormatter();
            case { } when propertyType == typeof(float):
            case { } when propertyType == typeof(double):
            case { } when propertyType == typeof(decimal):
                return new DecimalPropertyFormatter();
            case { } when propertyType == typeof(bool):
                return new BooleanPropertyFormatter();
            default:
                return new NoFormattingPropertyFormatter();
        }
    }
}
