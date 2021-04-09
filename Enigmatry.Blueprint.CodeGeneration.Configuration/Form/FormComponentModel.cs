using Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model;
using Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model.Select;
using Enigmatry.Blueprint.CodeGeneration.Configuration.Services;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Form
{
    public class FormComponentModel : IComponentModel, IWithLookupService
    {
        public ComponentInfo ComponentInfo { get; set; }
        public IList<FormControlModel> FormControls { get; set; }
        public string CreateOrUpdateCommandTypeName { get; set; }
        public LookupServiceModel LookupService { get; set; } = null!;


        public FormComponentModel(ComponentInfo componentInfo, IEnumerable<FormControlModel> formControls, string createOrUpdateCommandTypeName)
        {
            ComponentInfo = componentInfo;
            CreateOrUpdateCommandTypeName = createOrUpdateCommandTypeName;
            FormControls = formControls.ToList();

            if (SelectFormControls.Any())
            {
                LookupService = new LookupServiceModel
                {
                    Name = componentInfo.Name,
                    Methods = SelectFormControls
                        .Select(x => (SelectFormControlModel)x)
                        .Select(x => x.LookupMedhod)
                };
            }
        }

        public IEnumerable<FormControlModel> VisibleFormControls => FormControls.Where(control => control.IsVisible);
        public IEnumerable<FormControlModel> EditableFormControls => FormControls.Where(control => !control.IsReadonly);
        public IEnumerable<FormControlModel> AutocompleteFormControls => SelectFormControls.Where(x => x.Type == FormControlType.Autocomplete);


        private IEnumerable<FormControlModel> SelectFormControls => FormControls.Where(x => x is SelectFormControlModel);
    }
}
