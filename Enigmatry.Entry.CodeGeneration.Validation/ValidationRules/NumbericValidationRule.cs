﻿using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Enigmatry.Entry.CodeGeneration.Validation.Helpers;

namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public abstract class NumbericValidationRule<T> : ValidationRule<T>
    where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
{
    protected NumbericValidationRule(T rule, PropertyInfo propertyInfo, LambdaExpression expression, string message, string messageTranslationId)
        : base(rule, propertyInfo, expression, message, messageTranslationId)
    { }

    public string Increment => typeof(T).IsFloatingPointNumber() ? "0.1" : "1";

    public string RuleAsString => String.Format(CultureInfo.InvariantCulture, "{0}", Rule);
}