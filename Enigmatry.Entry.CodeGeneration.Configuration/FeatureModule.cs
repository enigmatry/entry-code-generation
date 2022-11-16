using System.Collections.Generic;
using System.Linq;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

namespace Enigmatry.Entry.CodeGeneration.Configuration
{
    public class FeatureModule : IFeatureModule
    {
        public string Name { get; }
        public IEnumerable<IComponentModel> Components { get; }
        public IEnumerable<ModuleImport> Imports { get; }
        public bool HasFormComponents => Components.OfType<FormComponentModel>().Any();
        public bool HasModuleImports => Imports.Any();

        public bool HasCommonValidationMessages =>
            Components.OfType<FormComponentModel>()
                .Any(form => form.ValidationRules.Any(rule => !rule.HasCustomMessage));

        public bool HasFormValidators =>
            Components.OfType<FormComponentModel>()
                .Any(form => form.FormControlsOfType<FormControl>().Any(control => control.Validators.Any()));

        public FeatureModule(string name, IEnumerable<IComponentModel> components)
        {
            Name = name;
            Components = components.ToList();
            Imports = Components.SelectMany(c => c.ComponentInfo.Feature.Imports).ToList();
        }

        public FeatureModule(IGrouping<string, IComponentModel> components)
            : this(components.Key, components)
        {
        }
    }
}
