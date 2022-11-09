using Enigmatry.CodeGeneration.Validation.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Validation.PropertyValidations
{
    public interface IPropertyValidation<T>
    {
        PropertyInfo PropertyInfo { get; }
        IList<IValidationRule> Rules { get; }
        void AddOrReplace(IValidationRule rule);
    }

    public interface IPropertyValidation<T, TProperty> : IPropertyValidation<T>
    {
        Expression<Func<T, TProperty>> PropertyExpression { get; }
    }
}
