using System;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class RadioGroupFormControlBuilder : BaseControlBuilder<RadioGroupFormControl, RadioGroupFormControlBuilder>
    {
        private readonly SelectOptionsBuilder _optionsBuilder = new SelectOptionsBuilder();

        public RadioGroupFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public RadioGroupFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public RadioGroupFormControlBuilder WithOptions(Action<SelectOptionsBuilder>? options = null)
        {
            options?.Invoke(_optionsBuilder);
            return this;
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var radioFormControl = new RadioGroupFormControl() { Options = _optionsBuilder.Build() };
            return Build(componentInfo, radioFormControl);
        }
    }
}
