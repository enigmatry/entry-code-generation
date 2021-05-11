using Enigmatry.CodeGeneration.Configuration.Formatters;
using Enigmatry.CodeGeneration.Configuration.List;

namespace Enigmatry.CodeGeneration.Tests.Mocks
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
                .IsSortable(false)
                .WithTranslationId("name");

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
