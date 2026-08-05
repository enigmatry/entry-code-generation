namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

/// <summary>
/// Client-side validation rule contract. The member names date back to the Formly-based
/// templates and are kept unchanged so implementations targeting the published package keep
/// compiling; template-engine code reads them through the framework-neutral
/// <see cref="ValidationRuleExtensions"/> accessors (<c>GetRuleName()</c> etc.).
/// </summary>
public interface IFormlyValidationRule : IBaseValidationRule
{
    /// <remarks>Prefer <see cref="ValidationRuleExtensions.GetValidationMessage"/> when reading.</remarks>
    string FormlyValidationMessage { get; }

    /// <remarks>Prefer <see cref="ValidationRuleExtensions.GetRuleName"/> when reading.</remarks>
    string FormlyRuleName { get; }

    /// <remarks>Prefer <see cref="ValidationRuleExtensions.GetTemplateOptions"/> when reading.</remarks>
    string[] FormlyTemplateOptions { get; }

    void SetMessageTranslationId(string messageTranslationId);
}
