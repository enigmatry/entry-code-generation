using Enigmatry.CodeGeneration.Configuration.Form;
using Enigmatry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.CodeGeneration.Tests.Angular.Mocks;
using Humanizer;
using NUnit.Framework;
using System.Linq;

namespace Enigmatry.CodeGeneration.Tests.Angular.HtmlHelperExtensions
{
    public class AngularMaterialHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
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

        [TestCase(nameof(FormMock.Name), ExpectedResult = "appearance: 'outline',")]
        [TestCase(nameof(FormMock.Description), ExpectedResult = "")]
        public string Appearance(string propertyName)
        {
            var formControl = _formComponent.FormControlsOfType<FormControl>().Single(x => x.PropertyName == propertyName.Camelize());
            return _htmlHelper.Appearance(formControl.Appearance)?.ToString()?.Replace("\r\n", "") ?? "";
        }

        [TestCase(nameof(FormMock.Name), ExpectedResult = "floatLabel: 'always',")]
        [TestCase(nameof(FormMock.Description), ExpectedResult = "")]
        public string FloatLabel(string propertyName)
        {
            var formControl = _formComponent.FormControlsOfType<FormControl>().Single(x => x.PropertyName == propertyName.Camelize());
            return _htmlHelper.FloatLabel(formControl.FloatLabel)?.ToString()?.Replace("\r\n", "") ?? "";
        }
    }
}
