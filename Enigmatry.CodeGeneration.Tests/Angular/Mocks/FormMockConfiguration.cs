using Enigmatry.CodeGeneration.Configuration.Form;
using Enigmatry.CodeGeneration.Configuration.Form.Model;

namespace Enigmatry.CodeGeneration.Tests.Angular.Mocks
{
    public class FormMockConfiguration : IFormComponentConfiguration<FormMock>
    {
        public void Configure(FormComponentBuilder<FormMock> builder)
        {
            builder
                .Component()
                .HasName("Form")
                .BelongsToFeature("Test");

            builder.FormControl(x => x.Id)
                .IsVisible(false);

            builder.FormControl(x => x.Name)
                .WithLabel("Name")
                .WithPlaceholder("Some / Name")
                .WithValidator("UniqueName")
                .WithValidator("IsEnsured")
                .WithCustomWrapper("tooltip")
                .WithTooltipText("Tooltip text")
                .WithAppearance(FormControlAppearance.Outline);

            builder.FormControl(x => x.Description)
                .WithLabel("Some Description")
                // no placeholder specified, should default to value of label
                .WithValidator("ValidDescription");

            builder
                .FormControl(x => x.Date)
                .WithLabel("Date")
                .WithPlaceholder("Date");

            builder
                .FormControl(x => x.Money)
                .WithLabel("Money")
                .WithPlaceholder("Money");

            builder
                .FormControl(x => x.Amount)
                .WithLabel("Amount")
                .WithPlaceholder("Amount");

            builder
                .FormControl(x => x.FormStatus)
                .IsDropDownListControl(options =>
                {
                    options.WithFixedValues<EnumMock>();
                    options.WithEmptyOption("None");
                });

            builder
                .FormControl(x => x.MockRadio)
                .WithLabel("Radio")
                .IsRadioGroupControl(options => options.WithFixedValues<EnumMock>());

            builder
                .FormControl(x => x.CategoryId)
                .WithLabel("Category")
                .IsDropDownListControl(options =>
                {
                    options.WithDynamicValues();
                    options.WithValueKey("id");
                    options.WithDisplayKey("categoryName");
                });

            builder
                .FormControl(x => x.TypeId)
                .WithLabel("Type")
                .IsDropDownListControl(options => options.WithDynamicValues());

            builder.FormControl(x => x.SubTypeId).IsVisible(false);

            builder
                .WithValidationConfiguration(new FormMockValidationConfiguration());
        }
    }
}
