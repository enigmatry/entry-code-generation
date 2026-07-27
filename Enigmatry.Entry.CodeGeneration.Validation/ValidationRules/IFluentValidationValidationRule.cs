using System.Linq.Expressions;

namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public interface IFluentValidationValidationRule : IBaseValidationRule
{
    LambdaExpression Expression { get; }
}
