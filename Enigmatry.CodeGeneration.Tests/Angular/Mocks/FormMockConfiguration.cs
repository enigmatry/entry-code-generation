using System;
using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Configuration.Form;
using Enigmatry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.CodeGeneration.Configuration.Form.Controls.Validators;

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
                .ButtonFormControl("ResetFormBtn")
                .WithLabel(String.Empty)
                .WithText("Reset")
                .WithClassName("primary-button");

            var formGroup = builder
                .FormControlGroup("Group Name")
                .CreateUiSection("group-type")
                .WithCustomWrapper("group-wrapper");

            formGroup
                .FormControl(x => x.Name)
                .WithLabel("Name")
                .WithPlaceholder("Some / Name")
                .WithValidator("UniqueName")
                .WithValidator("IsEnsured")
                .WithCustomWrapper("tooltip")
                .WithTooltipText("Tooltip text")
                .WithAppearance(FormControlAppearance.Outline)
                .WithFloatLabel(FormControlFloatLabel.Always)
                .WithUpdateOn(ValueUpdateTrigger.OnBlur);

            formGroup
                .TextareaFormControl(x => x.Description)
                .WithLabel("Some Description")
                .WithValidator("ValidDescription")
                .WithUpdateOn(ValueUpdateTrigger.OnBlur);

            formGroup
                .FormControl(x => x.Date)
                .WithCustomWrappers("tooltip", "form-field")
                .WithLabel("Date")
                .WithPlaceholder("Date")
                .IsReadonly(true);

            formGroup
                .FormControl(x => x.Money)
                .WithLabel("Money")
                .WithPlaceholder("Money");

            formGroup
                .FormControl(x => x.Amount)
                .WithLabel("Amount")
                .WithPlaceholder("Amount");

            formGroup
                .SelectFormControl(x => x.FormStatus)
                .WithOptions(options =>
                {
                    options.WithFixedValues<EnumMock>();
                    options.WithEmptyOption("None");
                    options.WithSortKey("displayName");
                });

            formGroup
                .RadioGroupFormControl(x => x.MockRadio)
                .WithLabel("Radio")
                .WithOptions(options => options.WithFixedValues<EnumMock>());

            formGroup
                .SelectFormControl(x => x.CategoryId)
                .WithLabel("Category")
                .WithOptions(options =>
                {
                    options.WithDynamicValues();
                    options.WithValueKey("id");
                    options.WithDisplayKey("categoryName");
                    options.WithSortKey("categoryName");
                });

            formGroup
                .MultiSelectFormControl(x => x.Types)
                .WithLabel("Types")
                .WithOptions(options =>
                {
                    options.WithDynamicValues();
                    options.WithSelectAllOption("SelectAll");
                    options.WithSortKey("value");
                });

            formGroup
                .FormControl(x => x.SubTypeId)
                .Ignore();

            formGroup
                .ArrayFormControl(x => x.Addresses)
                .WithCustomControlType("array-field")
                .WithCustomWrapper("array-wrapper")
                .WithItemConfiguration(config =>
                    {
                        config
                            .WithCustomWrapper("group-wrapper");
                        config
                            .FormControl(x => x.Id)
                            .IsVisible(false);
                        config
                            .InputFormControl(x => x.City)
                            .WithDefaultValue("Amsterdam");
                        config
                            .InputFormControl(x => x.Street);
                        config
                            .InputFormControl(x => x.HouseNumber);
                        config
                            .CheckboxFormControl(x => x.Verified)
                            .IsReadonly(true);
                    }
                );

            builder
                .WithValidationConfiguration(new FormMockValidationConfiguration());
        }
    }
}
