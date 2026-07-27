using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public class IsRequiredValidationRule : ValidationRule<bool>
{
    public IsRequiredValidationRule(PropertyInfo propertyInfo, LambdaExpression expression)
        : base(true, propertyInfo, expression, String.Empty, "validators.required")
    { }

    public override string RuleName => "required";

    public override string[] TemplateOptions =>
        new[] { $"{RuleName}: {Rule.ToString().ToLowerInvariant()}" };

    public override string ValidationMessage => HasCustomMessage
        ? CustomMessage
        : "${field?.templateOptions?.label}:property-name: is required";
}