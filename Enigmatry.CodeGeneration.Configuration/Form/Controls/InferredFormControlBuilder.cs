﻿using System;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class InferredFormControlBuilder : BaseControlBuilder<FormControl, InferredFormControlBuilder>
    {
        public InferredFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var propertyInfo = PropertyInfo!;
            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            switch (propertyType)
            {
                case { } when propertyType == typeof(DateTime):
                case { } when propertyType == typeof(DateTimeOffset):
#if NET6_0
                case { } when propertyType == typeof(DateOnly):
#endif
                    return Build(componentInfo, new DatepickerFormControl());
                case { } when propertyType == typeof(bool):
                    return Build(componentInfo, new CheckboxFormControl());
                default:
                    return Build(componentInfo, new InputFormControl());
            }
        }
    }
}
