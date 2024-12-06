using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

namespace Enigmatry.Entry.CodeGeneration.Configuration;

public static class ValidationExtensions
{
    /// <summary>
    /// Gets validation rules for type
    /// </summary>
    /// <param name="validationRules"></param>
    /// <returns></returns>
    public static IEnumerable<IFormlyValidationRule> ValidationRulesOfType<TValidationRule>(this IEnumerable<IFormlyValidationRule> validationRules) 
        where TValidationRule : IValidationRule => validationRules.OfType<TValidationRule>().Select(x => x as IFormlyValidationRule);

    /// <summary>
    /// Has validation rule for type
    /// </summary>
    /// <param name="validationRules"></param>
    /// <returns></returns>
    public static bool HasRule<TValidationRule>(this IEnumerable<IFormlyValidationRule> validationRules) where TValidationRule : IValidationRule =>
        validationRules.ValidationRulesOfType<TValidationRule>().Any();
}