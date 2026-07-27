using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public class LessOrEqualToValidationRule<T> : NumbericValidationRule<T>
    where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
{
    public LessOrEqualToValidationRule(T value, PropertyInfo propertyInfo, LambdaExpression expression)
        : base(value, propertyInfo, expression, String.Empty, "validators.max")
    { }

    public override string RuleName => "max";

    public override string[] TemplateOptions =>
        new[]
        {
            "type: 'number'",
            $"{RuleName}: {RuleAsString}"
        };

    public override string ValidationMessage => HasCustomMessage
        ? CustomMessage
        : "${field?.templateOptions?.label}:property-name: value should be less than ${field?.templateOptions?.max}:max-value:";
}