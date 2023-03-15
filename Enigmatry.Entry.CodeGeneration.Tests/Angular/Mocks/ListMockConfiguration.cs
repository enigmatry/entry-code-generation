using System.Collections.Generic;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;
using Enigmatry.Entry.CodeGeneration.Configuration.List;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;

public class ListMockConfiguration : IListComponentConfiguration<ListMock.Item>
{
    public void Configure(ListComponentBuilder<ListMock.Item> builder)
    {
        builder
            .Component()
            .HasName("MockList")
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