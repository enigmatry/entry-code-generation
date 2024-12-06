using Enigmatry.Entry.CodeGeneration.Validation.Helpers;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Validation.PropertyValidations;

public interface IPropertyValidationBuilder<T, TProperty> : IInitialPropertyValidationBuilder<T, TProperty>
{
    public IPropertyValidationBuilder<T, TProperty> WithMessage(string message, string messageTranslationId = "");
}

public class PropertyValidationBuilder<T, TProperty> : BasePropertyValidationBuilder<T, TProperty>, IPropertyValidationBuilder<T, TProperty>
{
    public PropertyValidationBuilder(IPropertyValidation<T, TProperty> propertyRule) : base(propertyRule) { }

    public IPropertyValidationBuilder<T, TProperty> WithMessage(string message, string messageTranslationId = "")
    {
        if (CurrentValidationRule == null)
        {
            throw new NullReferenceException("Current validation rule not selected");
        }

        Check.IfEmpty(message, $"{CurrentValidationRule.PropertyName.Pascalize()} validation message cannot be empty.");

        CurrentValidationRule.SetCustomMessage(message);

        if (!String.IsNullOrWhiteSpace(messageTranslationId))
        {
            CurrentValidationRule.SetMessageTranslationId(messageTranslationId);
        }

        return this;
    }
}
