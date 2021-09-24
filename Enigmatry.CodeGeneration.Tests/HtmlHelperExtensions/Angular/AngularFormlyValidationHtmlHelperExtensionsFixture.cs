using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.Form;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Validators;
using Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.CodeGeneration.Tests.Mocks;
using Humanizer;
using NUnit.Framework;
using System;
using System.Linq;

namespace Enigmatry.CodeGeneration.Tests.HtmlHelperExtensions.Angular
{
    public class AngularFormlyValidationHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
    {
        private FormComponentModel _formComponent = null!;
        private FeatureModule _featureModule = null!;

        [SetUp]
        public void SetUp()
        {
            _formComponent = new FormComponentModel(
                new ComponentInfo("form", String.Empty, RoutingInfo.NoRouting(), ApiClientInfo.NoApiClient(), new FeatureInfoBuilder().WithName("module").Build()),
                new FormComponentBuilder<FormMock>().Build().FormControls,
                new FormMockValidationConfiguration().ValidationRules
            );
            _formComponent.FormControls
                .Single(x => x.PropertyName == nameof(FormMock.Name).Camelize())
                .Validator = new CustomValidator("nameValidator");
            _featureModule = new FeatureModule("module", new[] { _formComponent });
        }

        [TestCase(nameof(FormMock.Name), ExpectedResult = "required: true,maxLength: 50,pattern: /[A-Z]/,")]
        [TestCase(nameof(FormMock.Amount), ExpectedResult = "required: true,type: 'number',min: 1,max: 99,")]
        [TestCase(nameof(FormMock.Email1), ExpectedResult = "pattern: /^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$/,")]
        [TestCase(nameof(FormMock.Email2), ExpectedResult = "pattern: /^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$/,")]
        public string AddValidationTemplateOptions(string propertyName)
        {
            var formControl = _formComponent.FormControls.Single(x => x.PropertyName == propertyName.Camelize());
            return _htmlHelper.AddValidationTemplateOptions(formControl)?.ToString()?.Replace("\r\n", "") ?? "";
        }

        [TestCase(nameof(FormMock.Name), ExpectedResult = "modelOptions: { updateOn: 'blur' },")]
        [TestCase(nameof(FormMock.Amount), ExpectedResult = "")]
        [TestCase(nameof(FormMock.Email1), ExpectedResult = "")]
        [TestCase(nameof(FormMock.Email1), ExpectedResult = "")]
        public string AddModelOpetions(string propertyName)
        {
            var formControl = _formComponent.FormControls.Single(x => x.PropertyName == propertyName.Camelize());
            return _htmlHelper.AddModelOpetions(formControl)?.ToString()?.Replace("\r\n", "") ?? "";
        }

        [TestCase(nameof(FormMock.Name), ExpectedResult = "asyncValidators: { validation: [ 'nameValidator' ] },")]
        [TestCase(nameof(FormMock.Amount), ExpectedResult = "")]
        [TestCase(nameof(FormMock.Email1), ExpectedResult = "")]
        [TestCase(nameof(FormMock.Email1), ExpectedResult = "")]
        public string AddAsyncValidators(string propertyName)
        {
            var formControl = _formComponent.FormControls.Single(x => x.PropertyName == propertyName.Camelize());
            return _htmlHelper.AddAsyncValidators(formControl)?.ToString()?.Replace("\r\n", "") ?? "";
        }

        [TestCase(nameof(FormMock.Name), ExpectedResult =
            "required: $localize `:@@CUSTOM_VALIDATION_MESSAGE_TRANSLATION_ID:CUSTOM_VALIDATION_MESSAGE`," +
            "maxlength: $localize `:@@CUSTOM_VALIDATION_MESSAGE_TRANSLATION_ID:CUSTOM_VALIDATION_MESSAGE`")]
        [TestCase(nameof(FormMock.Amount),ExpectedResult =
            "min: $localize `:@@module.form.amount.min:CUSTOM_VALIDATION_MESSAGE`," +
            "max: $localize `:@@module.form.amount.max:CUSTOM_VALIDATION_MESSAGE`")]
        [TestCase(nameof(FormMock.Email1), ExpectedResult = "pattern: $localize `:@@module.form.email1.pattern:CUSTOM_VALIDATION_MESSAGE`")]
        [TestCase(nameof(FormMock.Email2), ExpectedResult = "pattern: $localize `:@@validators.pattern.emailAddress:Invalid email address format`")]
        public string AddCustomValidationMessages(string propertyName)
        {
            var formControl = _formComponent.FormControls.Single(x => x.PropertyName == propertyName.Camelize());
            return _htmlHelper.AddCustomValidationMessages(formControl, true)?.ToString()?.Replace("\r\n", "") ?? "";
        }

        [TestCase(ExpectedResult =
            "{ name: 'pattern', message: (err, field) => $localize `:@@validators.pattern:${field?.templateOptions?.label}:property-name: is not in valid format` }," +
            "{ name: 'required', message: (err, field) => $localize `:@@validators.required:${field?.templateOptions?.label}:property-name: is required` }")]
        public string AddCommonValidationMessages() =>
            _htmlHelper.AddCommonValidationMessages(_featureModule, true)?.ToString()?.Replace("\r\n", "") ?? "";

        [TestCase(ExpectedResult = "import { CustomValidatorsService, customValidatorsFactory } from 'src/app/shared/validators/custom-validators';")]
        public string ImportValidators() =>
            _htmlHelper.ImportValidators(_featureModule)?.ToString()?.Replace("\r\n", "") ?? "";

        [TestCase(ExpectedResult = "CustomValidatorsService,{ provide: FORMLY_CONFIG, multi: true, useFactory: customValidatorsFactory, deps: [ CustomValidatorsService ] }")]
        public string AddFromValidationProvider() =>
            _htmlHelper.AddFromValidationProvider(_featureModule)?.ToString()?.Replace("\r\n", "") ?? "";
    }
}
