using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Configuration.Form;
using Enigmatry.CodeGeneration.Configuration.Form.Controls;

namespace Enigmatry.CodeGeneration.Tests.Angular.Mocks
{
    public class FormMockConfiguration : IFormComponentConfiguration<FormMock>
    {
        public void Configure(FormComponentBuilder<FormMock> builder)
        {
            builder
                .Component()
                .HasName("Form")
                .BelongsToFeature("Test")
                .OrderBy(OrderByType.Configuration);

            builder
                .FormControl(x => x.Id)
                .IsVisible(false);

            builder
                .FormControl(x => x.Name)
                .WithLabel("Name")
                .WithPlaceholder("Some / Name")
                .WithValidator("UniqueName")
                .WithValidator("IsEnsured")
                .WithCustomWrapper("tooltip")
                .WithTooltipText("Tooltip text")
                .WithAppearance(FormControlAppearance.Outline)
                .WithFloatLabel(FormControlFloatLabel.Always);

            builder
                .TextareaFormControl(x => x.Description)
                .WithLabel("Some Description")
                .WithValidator("ValidDescription");

            builder
                .FormControl(x => x.Date)
                .WithLabel("Date")
                .WithPlaceholder("Date")
                .IsReadonly(true);

            builder
                .FormControl(x => x.Money)
                .WithLabel("Money")
                .WithPlaceholder("Money");

            builder
                .FormControl(x => x.Amount)
                .WithLabel("Amount")
                .WithPlaceholder("Amount");

            builder
                .SelectFormControl(x => x.FormStatus)
                .WithOptions(options =>
                {
                    options.WithFixedValues<EnumMock>();
                    options.WithEmptyOption("None");
                });

            builder
                .RadioGroupFormControl(x => x.MockRadio)
                .WithLabel("Radio")
                .WithOptions(options => options.WithFixedValues<EnumMock>());

            builder
                .SelectFormControl(x => x.CategoryId)
                .WithLabel("Category")
                .WithOptions(options =>
                {
                    options.WithDynamicValues();
                    options.WithValueKey("id");
                    options.WithDisplayKey("categoryName");
                });

            builder
                .MultiSelectFormControl(x => x.Types)
                .WithLabel("Types")
                .WithOptions(options =>
                {
                    options.WithDynamicValues();
                    options.WithSelectAllOption("SelectAll");
                });

            builder
                .FormControl(x => x.SubTypeId)
                .Ignore();

            _ = builder
                .FormControlGroup("Group Name")
                .CreateUiSection("group-type")
                .WithCustomWrapper("group-wrapper");

            builder
                .WithValidationConfiguration(new FormMockValidationConfiguration());
        }
    }
}
