﻿using Enigmatry.CodeGeneration.Configuration.Form;
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

            builder.TextareaFormControl(x => x.Description)
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
                .SelectFormControl(x => x.TypeId)
                .WithLabel("Type")
                .WithOptions(options => options.WithDynamicValues());

            builder.FormControl(x => x.SubTypeId).IsVisible(false);

            builder
                .WithValidationConfiguration(new FormMockValidationConfiguration());
        }
    }
}
