using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public class MaxLengthValidationRule : ValidationRule<int>
{
    public MaxLengthValidationRule(int value, PropertyInfo propertyInfo, LambdaExpression expression)
        : base(value, propertyInfo, expression, String.Empty, "validators.maxLength")
    { }

    public override string RuleName => "maxLength";

    public override string ValidationMessage => HasCustomMessage
        ? CustomMessage
        : "${field?.templateOptions?.label}:property-name: value should be less than ${field?.templateOptions?.maxLength}:max-value: characters";
}