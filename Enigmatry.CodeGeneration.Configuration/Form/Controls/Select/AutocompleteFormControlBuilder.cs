using System;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class AutocompleteFormControlBuilder : BaseControlBuilder<AutocompleteFormControl, AutocompleteFormControlBuilder>
    {
        private readonly SelectOptionsBuilder _optionsBuilder = new SelectOptionsBuilder();

        public AutocompleteFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public AutocompleteFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public AutocompleteFormControlBuilder WithOptions(Action<SelectOptionsBuilder>? options = null)
        {
            options?.Invoke(_optionsBuilder);
            return this;
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var autocompleteFormControl = new AutocompleteFormControl() { Options = _optionsBuilder.Build() };
            return Build(componentInfo, autocompleteFormControl);
        }
    }
}
