using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.List;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Template;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions
{
    internal class TemplateHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
    {
        private FormComponentModel _formComponent = null!;
        private ListComponentModel _listComponent = null!;

        [SetUp]
        public void SetUp()
        {
            var formComponentConfiguration = new FormMockConfiguration();
            var formBuilder = new FormComponentBuilder<FormMock>();
            formComponentConfiguration.Configure(formBuilder);
            _formComponent = formBuilder.Build();

            var listComponentConfiguration = new ListMockConfiguration();
            var listBuilder = new ListComponentBuilder<ListMock.Item>();
            listComponentConfiguration.Configure(listBuilder);
            _listComponent = listBuilder.Build();
        }

        [TestCase(ExpectedResult = "class=\"entry-mock-edit-form entry-form\"")]
        public string FormCssClass() => _htmlHelper.FormCssClass(_formComponent)?.ToString() ?? "";

        [TestCase(ExpectedResult = "class=\"entry-mock-list-table entry-table\"")]
        public string ListCssClass() => _htmlHelper.ListCssClass(_listComponent)?.ToString() ?? "";
    }
}
