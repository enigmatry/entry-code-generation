using Enigmatry.BuildingBlocks.Validation;

namespace Enigmatry.CodeGeneration.Tests.Mocks
{
    public class FormMockValidationConfiguration : ValidationConfiguration<FormMock>
    {
        public FormMockValidationConfiguration()
        {
            RuleFor(x => x.Name)
                .IsRequired()
                .HasMaxLength(50, "Name length is too long");

            RuleFor(x => x.Amount)
                .IsRequired()
                .HasMinLength(0, "Amount must be greater then 0")
                .HasMaxLength(100, "Amount must be less then 100");
        }
    }
}
