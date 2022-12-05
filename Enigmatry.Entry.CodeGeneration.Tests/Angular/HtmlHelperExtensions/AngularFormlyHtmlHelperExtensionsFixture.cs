using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.List;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using Humanizer;
using NUnit.Framework;
using System.Linq;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions
{
    internal class AngularFormlyHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
    {
        private FormComponentModel _formComponent = null!;

        [SetUp]
        public void SetUp()
        {
            var componentConfiguration = new FormMockConfiguration();
            var builder = new FormComponentBuilder<FormMock>();
            componentConfiguration.Configure(builder);

            _formComponent = builder.Build();
        }

        [TestCase(nameof(FormMock.Name), ExpectedResult = "className: 'entry-name-field entry-input',\r\n")]
        [TestCase(nameof(FormMock.Description), ExpectedResult = "className: 'entry-description-field entry-textarea',\r\n")]
        [TestCase(nameof(FormMock.Date), ExpectedResult = "className: 'entry-date-field entry-datepicker',\r\n")]
        [TestCase(nameof(FormMock.CategoryId), ExpectedResult = "className: 'entry-category-id-field entry-select',\r\n")]
        [TestCase(nameof(FormMock.Money), ExpectedResult = "className: 'entry-money-field entry-input',\r\n")]
        [TestCase(nameof(FormMock.Amount), ExpectedResult = "className: 'entry-amount-field entry-input',\r\n")]
        [TestCase(nameof(FormMock.FormStatus), ExpectedResult = "className: 'entry-form-status-field entry-select',\r\n")]
        [TestCase(nameof(FormMock.MockRadio), ExpectedResult = "className: 'entry-mock-radio-field entry-radio',\r\n")]
        [TestCase(nameof(FormMock.Types), ExpectedResult = "className: 'entry-types-field entry-select',\r\n")]
        [TestCase(nameof(FormMock.MockMultiCheckbox), ExpectedResult = "className: 'entry-mock-multi-checkbox-field entry-multicheckbox',\r\n")]
        public string FieldCssClass(string propertyName)
        {
            var formControl = _formComponent
                .FormControlsOfType<FormControl>()
                .Single(x => x.PropertyName == propertyName.Camelize());
            return _htmlHelper.FieldCssClass(formControl)?.ToString() ?? "";
        }

        [TestCase(ExpectedResult = "fieldGroupClassName: 'entry-field-group',\r\n")]
        public string GroupCssClass()
        {
            var formControlGroup = _formComponent
                .FormControlsOfType<FormControlGroup>()
                .First();
            return _htmlHelper.GroupCssClass(formControlGroup)?.ToString() ?? "";
        }
    }
}
