namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

/// <summary>
/// Framework-neutral accessors for <see cref="IFormlyValidationRule"/>. The interface keeps its
/// original Formly-era member names for compatibility with published-package implementations;
/// new code reads rules through these extensions instead.
/// </summary>
public static class ValidationRuleExtensions
{
    public static string GetRuleName(this IFormlyValidationRule validationRule) => validationRule.FormlyRuleName;

    public static string GetValidationMessage(this IFormlyValidationRule validationRule) => validationRule.FormlyValidationMessage;

    public static string[] GetTemplateOptions(this IFormlyValidationRule validationRule) => validationRule.FormlyTemplateOptions;
}
