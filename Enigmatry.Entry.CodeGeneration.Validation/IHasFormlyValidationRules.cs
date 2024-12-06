using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

namespace Enigmatry.Entry.CodeGeneration.Validation;

public interface IHasFormlyValidationRules
{
    IEnumerable<IFormlyValidationRule> ValidationRules { get; }
}
