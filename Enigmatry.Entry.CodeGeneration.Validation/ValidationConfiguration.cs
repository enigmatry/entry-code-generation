using System.Linq.Expressions;
using Enigmatry.Entry.CodeGeneration.Validation.PropertyValidations;
using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

namespace Enigmatry.Entry.CodeGeneration.Validation;

public abstract class ValidationConfiguration<T> : IHasFormlyValidationRules where T : class
{
    private IList<IPropertyValidation<T>> PropertyValidations { get; set; } = new List<IPropertyValidation<T>>();

    public IEnumerable<IFormlyValidationRule> ValidationRules => PropertyValidations
        .SelectMany(propertyValidation => propertyValidation.Rules);

    public InitialPropertyValidationBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        var propertyValidator = new PropertyValidation<T, TProperty>(propertyExpression);
        AddOrUpdate(propertyValidator);
        return new InitialPropertyValidationBuilder<T, TProperty>(propertyValidator);
    }

    private void AddOrUpdate(IPropertyValidation<T> propertyValidator)
    {
        var existing = PropertyValidations.SingleOrDefault(x => x.PropertyInfo.Name == propertyValidator.PropertyInfo.Name);
        if (existing == null)
        {
            PropertyValidations.Add(propertyValidator);
        }
    }
}
