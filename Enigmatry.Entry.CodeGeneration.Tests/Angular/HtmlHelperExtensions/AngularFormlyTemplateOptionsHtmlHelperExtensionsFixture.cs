using System.Linq;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using Humanizer;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

public class AngularFormlyTemplateOptionsHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
{
    private FormComponentModel _formComponent = null!;

    [SetUp]
    public void SetUp()
    {
        var componentBuilder = new FormMockConfiguration();
        var builder = new FormComponentBuilder<FormMock>();
        componentBuilder.Configure(builder);

        _formComponent = builder.Build();
    }

    [TestCase(nameof(FormMock.Name), ExpectedResult = "")]
    [TestCase(nameof(FormMock.CategoryId), ExpectedResult = "metadata: { entityType: 'Category', filter: 'category_name' },")]
    public string Metadata(string propertyName)
    {
        var formControl = _formComponent.FormControlsOfType<FormControl>().Single(x => x.PropertyName == propertyName.Camelize());
        return _htmlHelper.AddMetadata(formControl.Metadata)?.ToString()?.Replace("\r\n", "") ?? "";
    }
}