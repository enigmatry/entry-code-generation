﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Validation.ValidationRules
{
    public class LessOrEqualToValidationRule<T> : NumbericValidationRule<T>
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        public LessOrEqualToValidationRule(T value, PropertyInfo propertyInfo, LambdaExpression expression)
            : base(value, propertyInfo, expression, String.Empty, "validators.max")
        { }

        public override string FormlyRuleName => "max";

        public override string[] FormlyTemplateOptions =>
            new[]
            {
                "type: 'number'",
                $"{FormlyRuleName}: {RuleAsString}"
            };

        public override string FormlyValidationMessage => HasCustomMessage
            ? CustomMessage
            : "${field?.templateOptions?.label}:property-name: value should be less than ${field?.templateOptions?.max}:max-value:";
    }
}
