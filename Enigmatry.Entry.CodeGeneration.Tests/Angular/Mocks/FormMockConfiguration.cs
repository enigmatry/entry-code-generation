using System;
using System.Collections.Generic;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;

public class FormMockConfiguration : IFormComponentConfiguration<FormMock>
{
    public void Configure(FormComponentBuilder<FormMock> builder)
    {
        builder
            .Component()
            .HasName("MockEdit")
            .BelongsToFeature("Test")
            .OrderBy(OrderByType.Configuration);
            
        builder
            .InputFormControl(x => x.Id)
            .IsVisible(false);

        builder
            .ButtonFormControl("ResetFormBtn")
            .WithLabel(String.Empty)
            .WithText("Reset")
            .WithClassName("primary-button");

        var formGroup = builder
            .FormControlGroup("Group Name")
            .CreateUiSection("group-type")
            .WithCustomWrapper("group-wrapper")
            .WithClassName("group-wrapper-readonly", ApplyWhen.FormIsReadonly);

        formGroup
            .InputFormControl(x => x.Name)
            .WithLabel("Name")
            .WithPlaceholder("Some / Name")
            .WithValidators("UniqueName", "IsEnsured")
            .WithCustomWrapper("tooltip")
            .WithTooltipText("Tooltip text")
            .WithDefaultValue("DEFAULT NAME")
            .WithAppearance(FormControlAppearance.Outline)
            .WithFloatLabel(FormControlFloatLabel.Always)
            .WithUpdateOn(ValueUpdateTrigger.OnBlur)
            .ShouldAutocomplete(false);

        formGroup
            .TextareaFormControl(x => x.Description)
            .WithLabel("Some Description")
            .WithValidators("ValidDescription")
            .WithUpdateOn(ValueUpdateTrigger.OnBlur);

        formGroup
            .DatepickerFormControl(x => x.Date)
            .WithCustomWrappers("tooltip", "form-field")
            .WithLabel("Date")
            .WithPlaceholder("Date")
            .IsReadonly(true);

        formGroup
            .InputFormControl(x => x.Money)
            .WithLabel("Money")
            .WithPlaceholder("Money");

        formGroup
            .InputFormControl(x => x.Amount)
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
            .MultiCheckboxFormControl(x => x.MockMultiCheckbox)
            .WithLabel("MultiCheckbox")
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
            })
            .WithMetadata(
                new KeyValuePair<string, string>("entityType", "Category"),
                new KeyValuePair<string, string>("filter", "category_name")
            );

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
            .InputFormControl(x => x.SubTypeId)
            .Ignore();

        formGroup
            .PasswordFormControl(x => x.Password);

        formGroup
            .CheckboxFormControl(x => x.IsActive)
            .WithLabel("Active")
            .WithDefaultValue(true);

        formGroup
            .ArrayFormControl(x => x.Addresses)
            .WithCustomControlType("array-field")
            .WithCustomWrapper("array-wrapper")
            .WithItemConfiguration(config =>
                {
                    config
                        .WithCustomWrapper("group-wrapper");
                    config
                        .InputFormControl(x => x.Id)
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
