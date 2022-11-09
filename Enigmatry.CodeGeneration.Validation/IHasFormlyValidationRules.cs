using Enigmatry.CodeGeneration.Validation.ValidationRules;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Validation
{
    public interface IHasFormlyValidationRules
    {
        IEnumerable<IFormlyValidationRule> ValidationRules { get; }
    }
}
