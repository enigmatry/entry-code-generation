using Enigmatry.BuildingBlocks.Validation;
using Enigmatry.BuildingBlocks.Validation.ValidationRules;
using Enigmatry.CodeGeneration.Configuration.Form.Model;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Select;
using Enigmatry.CodeGeneration.Configuration.Services;
using Humanizer;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.CodeGeneration.Configuration.Form
{
    public class FormComponentModel : IComponentModel, IWithLookupService
    {
        public ComponentInfo ComponentInfo { get; }
        public IList<FormControlModel> FormControls { get; set; }
        public LookupServiceModel? LookupService { get; set; }

        public FormComponentModel(
            ComponentInfo componentInfo,
            IEnumerable<FormControlModel> formControls,
            IHasValidationRules? validationConfirguration)
        {
            ComponentInfo = componentInfo;
            FormControls = formControls.ToList();

            ApplyValidationConfiguration(validationConfirguration);
            
            if (SelectFormControls.Any())
            {
                LookupService = new LookupServiceModel
                {
                    Name = componentInfo.Name,
                    Methods = SelectFormControls
                        .Select(x => (SelectFormControlModel)x)
                        .Select(x => x.LookupMethod)
                };
            }
        }

        public IEnumerable<FormControlModel> VisibleFormControls => FormControls.Where(control => control.IsVisible);
        private IEnumerable<FormControlModel> SelectFormControls => FormControls.Where(x => x is SelectFormControlModel);
        public bool OptionsAvailable(FormControlModel control) => control is SelectFormControlModel && LookupService != null;
        public bool HasCustomValidators =>
            FormControls.Any(control => control.CustomValidator != null) ||
            FormControls.Any(control => control.AsyncCustomValidator != null);

        private void ApplyValidationConfiguration(IHasValidationRules? validationConfirguration)
        {
            if (validationConfirguration == null)
                return;

            foreach (var formControl in FormControls)
            {
                SetValidationRulesToFormControl(formControl, validationConfirguration);
            }
        }

        private void SetValidationRulesToFormControl(FormControlModel formControl, IHasValidationRules validationConfirguration)
        {
            formControl.BuiltInValidationRules = validationConfirguration.BuiltInValidationRules
                .Where(x => x.PropertyName == formControl.PropertyName)
                .ToList();
            formControl.CustomValidator = validationConfirguration.ValidatorValidationRules
                .SingleOrDefault(x => x.PropertyName == formControl.PropertyName);
            formControl.AsyncCustomValidator = validationConfirguration.AsyncValidatorValidationRules
                .SingleOrDefault(x => x.PropertyName == formControl.PropertyName);

            SetTranslationIdsToValidationRules(formControl);
        }

        private void SetTranslationIdsToValidationRules(FormControlModel formControl)
        {
            foreach (var validationRule in formControl.BuiltInValidationRules)
            {
                TrySetDefaultMessageTranslationId(validationRule, formControl);
            }
            if (formControl.CustomValidator != null)
            {
                TrySetDefaultMessageTranslationId(formControl.CustomValidator, formControl);
            }
            if (formControl.AsyncCustomValidator != null)
            {
                TrySetDefaultMessageTranslationId(formControl.AsyncCustomValidator, formControl);
            }
        }

        private void TrySetDefaultMessageTranslationId(ValidationRule validationRule, FormControlModel formControl) =>
            validationRule.TrySetDefaultMessageTranslationId(
                    $"{ComponentInfo.Feature.Name.Kebaberize()}" +
                    $".{ComponentInfo.Name.Kebaberize()}" +
                    $".{formControl.PropertyName.Kebaberize()}" +
                    $".{validationRule.Name.Kebaberize()}");
    }
}
