using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public class GreaterThenValidationRule<T> : NumbericValidationRule<T>
    where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
{
    public GreaterThenValidationRule(T value, PropertyInfo propertyInfo, LambdaExpression expression)
        : base(value, propertyInfo, expression, String.Empty, "validators.min")
    { }

    public override string RuleName => "min";

    public override string[] TemplateOptions =>
        new[]
        {
            "type: 'number'",
            $"{RuleName}: {RuleAsString} + {Increment}"
        };

    public override string ValidationMessage => HasCustomMessage
        ? CustomMessage
        : "${field?.templateOptions?.label}:property-name: value should be more than ${field?.templateOptions?.min}:min-value:";
}