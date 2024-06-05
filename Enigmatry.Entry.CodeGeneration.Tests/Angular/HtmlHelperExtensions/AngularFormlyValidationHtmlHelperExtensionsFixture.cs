using System.Linq;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Validators;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using Humanizer;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

public class AngularFormlyValidationHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
{
    private FormComponentModel _formComponent = null!;
    private FeatureModule _featureModule = null!;

    [SetUp]
    public void SetUp()
    {
        var componentBuilder = new FormMockConfiguration();
        var builder = new FormComponentBuilder<FormMock>();
        componentBuilder.Configure(builder);

        _formComponent = builder.Build();

        _formComponent.FormControlsOfType<FormControl>()
            .Single(x => x.PropertyName == nameof(FormMock.Name).Camelize())
            .Validators = new[] { new CustomValidator("nameValidator") };
        _featureModule = new FeatureModule("module", new[] { _formComponent });
    }

    [TestCase(nameof(FormMock.Name), ExpectedResult = "required: true,maxLength: 50,pattern: /[A-Z]/,")]
    [TestCase(nameof(FormMock.Amount), ExpectedResult = "required: true,min: 0 + 1,max: 100,")]
    [TestCase(nameof(FormMock.Email1), ExpectedResult = "pattern: /^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$/,maxLength: 50,")]
    [TestCase(nameof(FormMock.Email2), ExpectedResult = "pattern: /^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$/,maxLength: 50,")]
    public string AddValidationTemplateOptions(string propertyName)
    {
        var formControl = _formComponent.FormControlsOfType<FormControl>().Single(x => x.PropertyName == propertyName.Camelize());
        return _htmlHelper.AddValidationTemplateOptions(formControl).ToString()?.Replace("\r\n", "") ?? "";
    }

    [TestCase(nameof(FormMock.Name), ExpectedResult = "modelOptions: { updateOn: 'blur' },")]
    [TestCase(nameof(FormMock.Money), ExpectedResult = "")]
    [TestCase(nameof(FormMock.Amount), ExpectedResult = "")]
    [TestCase(nameof(FormMock.Date), ExpectedResult = "modelOptions: { updateOn: 'blur' },")]
    public string AddModelOptions(string propertyName)
    {
        var formControl = _formComponent.FormControlsOfType<FormControl>().Single(x => x.PropertyName == propertyName.Camelize());
        return _htmlHelper.AddModelOptions(formControl).ToString()?.Replace("\r\n", "") ?? "";
    }

    [TestCase(nameof(FormMock.Name), ExpectedResult = "asyncValidators: { validation: [ 'nameValidator' ] },")]
    [TestCase(nameof(FormMock.Money), ExpectedResult = "")]
    [TestCase(nameof(FormMock.Amount), ExpectedResult = "")]
    [TestCase(nameof(FormMock.Email1), ExpectedResult = "")]
    [TestCase(nameof(FormMock.Email1), ExpectedResult = "")]
    public string AddAsyncValidators(string propertyName)
    {
        var formControl = _formComponent.FormControlsOfType<FormControl>().Single(x => x.PropertyName == propertyName.Camelize());
        return _htmlHelper.AddAsyncValidators(formControl).ToString()?.Replace("\r\n", "") ?? "";
    }

    [TestCase(nameof(FormMock.Name), ExpectedResult =
        "required: (err, field) => $localize `:@@CUSTOM_VALIDATION_MESSAGE_TRANSLATION_ID:CUSTOM_VALIDATION_MESSAGE`," +
        "maxLength: (err, field) => $localize `:@@CUSTOM_VALIDATION_MESSAGE_TRANSLATION_ID:CUSTOM_VALIDATION_MESSAGE`")]
    [TestCase(nameof(FormMock.Amount), ExpectedResult =
        "min: (err, field) => $localize `:@@test.mock-edit.amount.min:CUSTOM_VALIDATION_MESSAGE`," +
        "max: (err, field) => $localize `:@@test.mock-edit.amount.max:CUSTOM_VALIDATION_MESSAGE`")]
    [TestCase(nameof(FormMock.Email1), ExpectedResult = "pattern: (err, field) => $localize `:@@test.mock-edit.email1.pattern:CUSTOM_VALIDATION_MESSAGE`")]
    [TestCase(nameof(FormMock.Email2), ExpectedResult = "pattern: (err, field) => $localize `:@@validators.pattern.emailAddress:Invalid email address format`")]
    public string AddCustomValidationMessages(string propertyName)
    {
        var formControl = _formComponent.FormControlsOfType<FormControl>().Single(x => x.PropertyName == propertyName.Camelize());
        return _htmlHelper.AddCustomValidationMessages(formControl, true).ToString()?.Replace("\r\n", "") ?? "";
    }

    [TestCase(ExpectedResult =
        "{ name: 'maxLength', message: (err, field) => $localize `:@@validators.maxLength:${field?.templateOptions?.label}:property-name: value should be less than ${field?.templateOptions?.maxLength}:max-value: characters` }," +
        "{ name: 'pattern', message: (err, field) => $localize `:@@validators.pattern:${field?.templateOptions?.label}:property-name: is not in valid format` }," +
        "{ name: 'required', message: (err, field) => $localize `:@@validators.required:${field?.templateOptions?.label}:property-name: is required` }")]
    public string AddCommonValidationMessages() =>
        _htmlHelper.AddCommonValidationMessages(_featureModule, true).ToString()?.Replace("\r\n", "") ?? "";

    [TestCase("src/app/custom-path", ExpectedResult = "import { CustomValidatorsService, customValidatorsFactory } from 'src/app/custom-path';")]
    public string ImportValidators(string path) =>
        _htmlHelper.ImportValidators(_featureModule, path).ToString()?.Replace("\r\n", "") ?? "";

    [TestCase(ExpectedResult = "CustomValidatorsService,{ provide: FORMLY_CONFIG, multi: true, useFactory: customValidatorsFactory, deps: [ CustomValidatorsService ] }")]
    public string AddFromValidationProvider() =>
        _htmlHelper.AddFromValidationProvider(_featureModule).ToString()?.Replace("\r\n", "") ?? "";
}
