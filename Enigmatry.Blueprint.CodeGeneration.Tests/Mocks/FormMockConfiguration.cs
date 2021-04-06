using Enigmatry.Blueprint.CodeGeneration.Configuration.Form;

namespace Enigmatry.Blueprint.CodeGeneration.Tests.Mocks
{
    public class FormMockConfiguration : IFormComponentConfiguration<FormMock>
    {
        public void Configure(FormComponentBuilder<FormMock> builder)
        {
            builder
                .Component()
                .HasName("Form")
                .BelongsToFeature("Test");

            builder
                .HasCreateOrUpdateCommandOfType<FormMockCommand>();

            builder.FormControl(x => x.Id)
                .IsVisible(false);

            builder.FormControl(x => x.Name)
                .WithLabel("Name")
                .WithPlaceholder("Name");

            builder.FormControl(x => x.Description)
                .WithLabel("Description")
                .WithPlaceholder("Description");

            builder
                .FormControl(x => x.Date)
                .WithLabel("Date")
                .WithPlaceholder("Date");

            builder
                .FormControl(x => x.Money)
                .WithLabel("Money")
                .WithPlaceholder("Money");

            builder
                .FormControl(x => x.Status)
                .WithLabel("Status")
                .IsDropDownListControl()
                .WithFixedValues<EnumMock>();

            builder
                .FormControl(x => x.CategoryId)
                .WithLabel("Category")
                .IsDropDownListControl()
                .WithCallbackMethod(typeof(LookupApiControllerMock).GetMethod(nameof(LookupApiControllerMock.GetCategoryLookups)));

            builder
                .FormControl(x => x.TypeId)
                .WithLabel("Type")
                .IsDropDownListControl()
                .WithCallbackMethod(typeof(LookupApiControllerMock).GetMethod(nameof(LookupApiControllerMock.GetTypeLookups)))
                .DependsOn(builder.FormControl(x => x.CategoryId));

            builder
                .FormControl(x => x.SubTypeId)
                .WithLabel("SubType")
                .IsDropDownListControl()
                .WithCallbackMethod(typeof(LookupApiControllerMock).GetMethod(nameof(LookupApiControllerMock.GetSubTypeLookups)))
                .DependsOn(builder.FormControl(x => x.TypeId));
        }
    }
}
