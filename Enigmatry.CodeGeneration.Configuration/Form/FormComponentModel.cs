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
            IEnumerable<IFormlyValidationRule> validationRules)
        {
            ComponentInfo = componentInfo;
            FormControls = formControls.ToList();

            ApplyValidationConfiguration(validationRules);
            
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

        private void ApplyValidationConfiguration(IEnumerable<IFormlyValidationRule> validationRules)
        {
            if (validationRules.Any())
            {
                FormControls.ToList()
                    .ForEach(formControl => SetValidationRulesToFormControl(formControl, validationRules));
            }
        }

        private void SetValidationRulesToFormControl(FormControlModel formControl, IEnumerable<IFormlyValidationRule> validationRules)
        {
            formControl.ValidationRules = validationRules
                .Where(x => x.PropertyName == formControl.PropertyName).ToList();

            formControl.ValidationRules
                .Where(x => !x.HasMessageTranslationId).ToList()
                .ForEach(validationRule => validationRule.SetMessageTranslationId(
                    $"{ComponentInfo.Feature.Name.Kebaberize()}" +
                    $".{ComponentInfo.Name.Kebaberize()}" +
                    $".{formControl.PropertyName.Kebaberize()}" +
                    $".{validationRule.FormlyRuleName.Kebaberize()}"
                ));
        }
    }
}
