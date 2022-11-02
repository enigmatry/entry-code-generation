using Enigmatry.Entry.Validation.ValidationRules;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.Form.Controls;

namespace Enigmatry.CodeGeneration.Configuration.Form
{
    public class FormComponentModel : IComponentModel
    {
        public ComponentInfo ComponentInfo { get; }
        public IList<FormControl> FormControls { get; }
        public IList<IFormlyValidationRule> ValidationRules { get; }

        public FormComponentModel(
            ComponentInfo componentInfo,
            IEnumerable<FormControl> formControls,
            IEnumerable<IFormlyValidationRule> validationRules)
        {
            ComponentInfo = componentInfo;
            FormControls = formControls.ToList();
            ValidationRules = validationRules.ToList();

            ApplyValidationConfiguration(ValidationRules);
        }

        private void ApplyValidationConfiguration(IList<IFormlyValidationRule> validationRules)
        {
            if (validationRules.Any())
            {
                FormControls.ToList()
                    .ForEach(formControl => formControl.ApplyValidationConfiguration(validationRules));
            }
        }

        public IEnumerable<TControl> FormControlsOfType<TControl>() where TControl : FormControl
        {
            return FormControls.OfType<TControl>().Concat(
                FormControls.OfType<FormControlGroup>()
                    .SelectMany(x => x.FormControlsOfType<TControl>()));
        }
    }
}
