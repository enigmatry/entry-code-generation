namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public interface IFormlyValidationRule : IBaseValidationRule
{
    string ValidationMessage { get; }
    string RuleName { get; }
    string[] TemplateOptions { get; }

    void SetMessageTranslationId(string messageTranslationId);
}
