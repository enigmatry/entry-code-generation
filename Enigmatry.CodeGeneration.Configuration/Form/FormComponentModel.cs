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
            IEnumerable<ValidationRule> validationRules)
        {
            ComponentInfo = componentInfo;
            FormControls = formControls.ToList();

            foreach (var formControl in FormControls)
            {
                SetValidationRulesToFormControl(formControl, validationRules);
            }

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
        public IEnumerable<FormControlModel> EditableFormControls => FormControls.Where(control => !control.IsReadonly);
        public IEnumerable<FormControlModel> AutocompleteFormControls => SelectFormControls.Where(x => x.Type == FormControlType.Autocomplete);
        private IEnumerable<FormControlModel> SelectFormControls => FormControls.Where(x => x is SelectFormControlModel);


        private void SetValidationRulesToFormControl(FormControlModel formControl, IEnumerable<ValidationRule> validationRules)
        {
            formControl.ValidationRules = validationRules
                .Where(x => x.PropertyName == formControl.PropertyName)
                .ToList();
            SetTranslationIdsToValidationRules(formControl);
        }

        private void SetTranslationIdsToValidationRules(FormControlModel formControl)
        {
            foreach (var validationRule in formControl.ValidationRules)
            {
                validationRule.TrySetMessageTranslationId(
                    $"{ComponentInfo.Feature.Name.Kebaberize()}" +
                    $".{ComponentInfo.Name.Kebaberize()}" +
                    $".{formControl.PropertyName.Kebaberize()}" +
                    $".{validationRule.Name.Kebaberize()}");
            }
        }
    }
}
