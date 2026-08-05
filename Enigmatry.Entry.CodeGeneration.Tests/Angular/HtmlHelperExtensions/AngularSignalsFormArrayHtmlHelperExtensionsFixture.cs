using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

internal sealed class AngularSignalsFormArrayHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
{
    [Test]
    public void ItemFactoryFlattensGroupChildren()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.ArrayFormControl(x => x.Addresses)
            .WithItemConfiguration(itemConfiguration =>
            {
                itemConfiguration.InputFormControl(x => x.City);
                var locationGroup = itemConfiguration.FormControlGroup("location");
                locationGroup.InputFormControl(x => x.Street);
                locationGroup.InputFormControl(x => x.HouseNumber);
            });
        var arrayControl = builder.Build().FormControls.OfType<ArrayFormControl>().Single();

        var itemFactory = _htmlHelper.ArrayItemFactoryMethod(arrayControl).ToString() ?? "";

        itemFactory.ShouldContain("city: new FormControl");
        itemFactory.ShouldContain("street: new FormControl");
        itemFactory.ShouldContain("houseNumber: new FormControl");
        itemFactory.ShouldNotContain("location:");
    }
}
