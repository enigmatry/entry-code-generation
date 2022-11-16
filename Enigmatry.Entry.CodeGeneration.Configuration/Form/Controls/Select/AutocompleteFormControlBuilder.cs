using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls
{
    public class AutocompleteFormControlBuilder : SelectControlBuilderBase<AutocompleteFormControl, AutocompleteFormControlBuilder>
    {
        public AutocompleteFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public AutocompleteFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var autocompleteFormControl = new AutocompleteFormControl { Options = _optionsBuilder.Build() };
            return Build(componentInfo, autocompleteFormControl);
        }
    }
}
