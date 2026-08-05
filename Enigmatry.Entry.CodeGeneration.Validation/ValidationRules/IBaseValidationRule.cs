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
