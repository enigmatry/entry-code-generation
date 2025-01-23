using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public class MinLengthValidationRule : ValidationRule<int>
{
    public MinLengthValidationRule(int value, PropertyInfo propertyInfo, LambdaExpression expression)
        : base(value, propertyInfo, expression, String.Empty, "validators.minLength")
    { }

    public override string FormlyRuleName => "minLength";

    public override string FormlyValidationMessage => HasCustomMessage
        ? CustomMessage
        : "${field?.props?.label}:property-name: should have at least ${field?.templateOptions?.minLength}:min-value: characters";
}