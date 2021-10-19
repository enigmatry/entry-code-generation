﻿using System.Collections.Generic;
using Enigmatry.CodeGeneration.Configuration.Formatters;
using Enigmatry.CodeGeneration.Configuration.List;

namespace Enigmatry.CodeGeneration.Tests.Angular.Mocks
{
    public class ListMockConfiguration : IListComponentConfiguration<ListMock.Item>
    {

        public void Configure(ListComponentBuilder<ListMock.Item> builder)
        {
            builder
                .Component()
                .HasName("List")
                .BelongsToFeature("Test");

            builder
                .Column(x => x.Id)
                .IsVisible(false);

            builder
                .Column(x => x.Name)
                .IsSortable(true).WithSortId("User.Name")
                .WithTranslationId(Constants.CustomTranslationId)
                .WithCustomComponent("custom-cell")
                .WithCustomCssClass("name-custom-class")
                .WithCustomProperties(new Dictionary<string, string> {{"CodeSystem", "ABC"}});

            builder
                .Column(x => x.Date)
                .WithFormat(
                    new DatePropertyFormatter()
                        .WithFormat("mediumDate")
                        .WithLocale("en")
                        .WithTimeZone("UTC")
                );
        }
    }
}