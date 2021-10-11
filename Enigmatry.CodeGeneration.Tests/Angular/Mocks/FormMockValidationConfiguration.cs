using System.Text.RegularExpressions;
using Enigmatry.BuildingBlocks.Validation;

namespace Enigmatry.CodeGeneration.Tests.Angular.Mocks
{
    public class FormMockValidationConfiguration : ValidationConfiguration<FormMock>
    {
        public FormMockValidationConfiguration()
        {
            RuleFor(x => x.Name)
                .IsRequired()
                    .WithMessage(Constants.CustomValidationMessage, Constants.CustomValidationMessageTranslationId)
                .MaxLength(50)
                    .WithMessage(Constants.CustomValidationMessage, Constants.CustomValidationMessageTranslationId)
                .Match(new Regex("/[A-Z]/"));

            RuleFor(x => x.Money)
                .LessThen(999.99M);

            RuleFor(x => x.Amount)
                .IsRequired()
                .GreaterThen(0)
                    .WithMessage(Constants.CustomValidationMessage)
                .LessOrEqualTo(100)
                    .WithMessage(Constants.CustomValidationMessage);

            RuleFor(x => x.Email1)
                .EmailAddress()
                    .WithMessage(Constants.CustomValidationMessage);

            RuleFor(x => x.Email2)
                .EmailAddress();
        }
    }
}
