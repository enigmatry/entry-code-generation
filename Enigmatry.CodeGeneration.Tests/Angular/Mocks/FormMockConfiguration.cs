using Enigmatry.CodeGeneration.Configuration.Form;

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
                .WithValidator("IsEnsured");

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
                //.WithLabel("Status")
                .IsDropDownListControl()
                .WithFixedValues<EnumMock>();

            builder
                .FormControl(x => x.MockRadio)
                .WithLabel("Radio")
                .IsRadioGroupControl()
                .WithFixedValues<EnumMock>();

            builder.FormControl(x => x.CategoryId).IsVisible(false);
            builder.FormControl(x => x.TypeId).IsVisible(false);
            builder.FormControl(x => x.SubTypeId).IsVisible(false);

            builder
                .WithValidationConfiguration(new FormMockValidationConfiguration());
        }
    }
}
