using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public interface IBaseValidationRule
{
    string CustomMessage { get; }
    string MessageTranslationId { get; }
    bool HasCustomMessage { get; }
    bool HasMessageTranslationId { get; }
    PropertyInfo PropertyInfo { get; }
    string PropertyName { get; }
}

public interface IFormlyValidationRule : IBaseValidationRule
{
    string ValidationMessage { get; }
    string RuleName { get; }
    string[] TemplateOptions { get; }

    void SetMessageTranslationId(string messageTranslationId);
}

public interface IFluentValidationValidationRule : IBaseValidationRule
{
    LambdaExpression Expression { get; }
}

public interface IValidationRule : IFormlyValidationRule, IFluentValidationValidationRule
{
    void SetCustomMessage(string message);
}