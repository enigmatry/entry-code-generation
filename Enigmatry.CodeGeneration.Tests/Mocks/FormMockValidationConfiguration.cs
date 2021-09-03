using Enigmatry.BuildingBlocks.Validation;

namespace Enigmatry.CodeGeneration.Tests.Mocks
{
    public class FormMockValidationConfiguration : AbstractValidationConfiguration<FormMock>
    {
        public FormMockValidationConfiguration()
        {
            RuleFor(x => x.Name)
                .IsRequired()
                    .WithMessageTranslationId(Constants.CustomValidationMessageTranslationId)
                .Max(50)
                    .WithMessage(Constants.CustomValidationMessage)
                    .WithMessageTranslationId(Constants.CustomValidationMessageTranslationId);

            RuleFor(x => x.Amount)
                .IsRequired()
                .Min(0)
                    .WithMessage(Constants.CustomValidationMessage)
                .Max(100)
                    .WithMessage(Constants.CustomValidationMessage);
        }
    }
}
