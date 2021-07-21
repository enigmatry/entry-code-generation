﻿using Enigmatry.CodeGeneration.Configuration.Form;

namespace Enigmatry.CodeGeneration.Tests.Mocks
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

            builder.FormControl(x => x.CategoryId).IsVisible(false);
            builder.FormControl(x => x.TypeId).IsVisible(false);
            builder.FormControl(x => x.SubTypeId).IsVisible(false);
        }
    }
}
