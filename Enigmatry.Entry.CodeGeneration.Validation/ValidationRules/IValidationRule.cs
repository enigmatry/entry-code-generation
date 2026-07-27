namespace Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

public interface IValidationRule : IFormlyValidationRule, IFluentValidationValidationRule
{
    void SetCustomMessage(string message);
}
