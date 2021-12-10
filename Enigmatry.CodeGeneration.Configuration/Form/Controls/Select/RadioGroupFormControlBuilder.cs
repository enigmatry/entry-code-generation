using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class RadioGroupFormControlBuilder : SelectControlBuilderBase<RadioGroupFormControl, RadioGroupFormControlBuilder>
    {
        public RadioGroupFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public RadioGroupFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var radioFormControl = new RadioGroupFormControl() { Options = _optionsBuilder.Build() };
            return Build(componentInfo, radioFormControl);
        }
    }
}
