namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public interface IFormlyValidationRule : IBaseValidationRule
{
    string ValidationMessage { get; }
    string RuleName { get; }
    string[] TemplateOptions { get; }

    [Obsolete("Use ValidationMessage instead.")]
    string FormlyValidationMessage { get; }

    [Obsolete("Use RuleName instead.")]
    string FormlyRuleName { get; }

    [Obsolete("Use TemplateOptions instead.")]
    string[] FormlyTemplateOptions { get; }

    void SetMessageTranslationId(string messageTranslationId);
}
