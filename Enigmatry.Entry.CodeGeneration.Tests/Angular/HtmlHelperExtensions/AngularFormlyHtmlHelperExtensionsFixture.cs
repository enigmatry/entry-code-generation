using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using FluentAssertions;
using Humanizer;
using NUnit.Framework;
using System;
using System.Linq;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

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

    [TestCase(nameof(FormMock.Name), ExpectedResult = "className: `entry-name-field entry-input`,\r\n")]
    [TestCase(nameof(FormMock.Description), ExpectedResult = "className: `entry-description-field entry-redactor`,\r\n")]
    [TestCase(nameof(FormMock.Date), ExpectedResult = "className: `entry-date-field entry-datepicker`,\r\n")]
    [TestCase(nameof(FormMock.CategoryId), ExpectedResult = "className: `entry-category-id-field entry-select`,\r\n")]
    [TestCase(nameof(FormMock.Money), ExpectedResult = "className: `entry-money-field entry-input`,\r\n")]
    [TestCase(nameof(FormMock.Amount), ExpectedResult = "className: `entry-amount-field entry-input`,\r\n")]
    [TestCase(nameof(FormMock.FormStatus), ExpectedResult = "className: `entry-form-status-field entry-select`,\r\n")]
    [TestCase(nameof(FormMock.MockRadio), ExpectedResult = "className: `entry-mock-radio-field entry-radio`,\r\n")]
    [TestCase(nameof(FormMock.Types), ExpectedResult = "className: `entry-types-field entry-select`,\r\n")]
    [TestCase(nameof(FormMock.MockMultiCheckbox), ExpectedResult = "className: `entry-mock-multi-checkbox-field entry-multicheckbox`,\r\n")]
    public string FieldCssClass(string propertyName)
    {
        var formControl = _formComponent
            .FormControlsOfType<FormControl>()
            .Single(x => x.PropertyName == propertyName.Camelize());
        return _htmlHelper.FieldCssClass(formControl).ToString() ?? "";
    }

    [TestCase(ExpectedResult = "fieldGroupClassName: `entry-field-group ${this.applyOptionally('group-wrapper-readonly', this.isReadonly)}`,\r\n")]
    public string GroupCssClass()
    {
        var formControlGroup = _formComponent
            .FormControlsOfType<FormControlGroup>()
            .First();
        return _htmlHelper.GroupCssClass(formControlGroup).ToString() ?? "";
    }

    [Test]
    public void DefaultValue()
    {
        var inputFormControl = _formComponent
            .FormControlsOfType<InputFormControl>()
            .Single(x => x.PropertyName == nameof(FormMock.Name).Camelize());
        _htmlHelper.DefaultValue(inputFormControl).ToString()
            .Should().Be("defaultValue: 'DEFAULT NAME',\r\n");

        var textAreaFormControl = _formComponent
            .FormControlsOfType<RichTextInputFormControl>()
            .Single(x => x.PropertyName == nameof(FormMock.Description).Camelize());
        textAreaFormControl.DefaultValue = "DEFAULT_TEXT_AREA";
        _htmlHelper.DefaultValue(textAreaFormControl).ToString()
            .Should().Be($"defaultValue: 'DEFAULT_TEXT_AREA',\r\n");

        var checkBoxFormControl = _formComponent
            .FormControlsOfType<CheckboxFormControl>()
            .Single(x => x.PropertyName == nameof(FormMock.IsActive).Camelize());
        _htmlHelper.DefaultValue(checkBoxFormControl).ToString()
            .Should().Be($"defaultValue: true,\r\n");

        var radioFormControl = _formComponent
            .FormControlsOfType<RadioGroupFormControl>()
            .Single(x => x.PropertyName == nameof(FormMock.MockRadio).Camelize());
        radioFormControl.DefaultValue = "DEFAULT_RADIO";
        _htmlHelper.DefaultValue(radioFormControl).ToString()
            .Should().Be($"defaultValue: 'DEFAULT_RADIO',\r\n");

        var selectFormControl = _formComponent
            .FormControlsOfType<SelectFormControl>()
            .Single(x => x.PropertyName == nameof(FormMock.FormStatus).Camelize());
        selectFormControl.DefaultValue = "DEFAULT_SELECT";
        _htmlHelper.DefaultValue(selectFormControl).ToString()
            .Should().Be($"defaultValue: 'DEFAULT_SELECT',\r\n");

        var datePickerFormControl = _formComponent
            .FormControlsOfType<DatepickerFormControl>()
            .Single(x => x.PropertyName == nameof(FormMock.Date).Camelize());
        datePickerFormControl.DefaultValue = new DateTimeOffset(2000, 1, 1, 12, 33, 15, TimeSpan.Zero);
        _htmlHelper.DefaultValue(datePickerFormControl).ToString()
            .Should().Be($"defaultValue: '2000-01-01T12:33:15.0000000+00:00',\r\n");
    }
}
