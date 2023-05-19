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
            .RichTextInputFormControl(x => x.Description)
            .WithEditor(RichTextEditor.Ckeditor)
            .WithLabel("Some Description");

        formGroup
            .DatepickerFormControl(x => x.Date)
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
            .WithPlaceholder("Amount")
            .WithHint("Don't panic if it is 0");

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
            .WithOptions(options => options.WithFixedValues<EnumMock>(false));

        formGroup
            .MultiCheckboxFormControl(x => x.MockMultiCheckbox)
            .WithLabel("MultiCheckbox")
            .WithOptions(options => options.WithFixedValues<EnumMock>());

        formGroup
            .MultiCheckboxFormControl(x => x.MultiCheckboxWithStringIds)
            .WithLabel("MultiCheckbox")
            .WithOptions(options => options.WithFixedValues(new[]
            {
                new SelectOption("value_1", "Label 1", "translation_Id_1"),
                new SelectOption(-9, "Label 3", "translation_Id_3"),
                new SelectOption(1234567, "Label 4", "translation_Id_4"),
                new SelectOption((int)EnumMock.Third, "Label 5", "translation_Id_5"),
                new SelectOption(null, "Label 6", "translation_Id_6")
            }));

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
            .FormControl(x => x.SubTypeId)
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

        formGroup.DateTimePickerFormControl(x => x.DateAndTime);

        builder
            .WithValidationConfiguration(new FormMockValidationConfiguration());
    }
}
