using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class MultiCheckboxFormControlBuilder : SelectControlBuilderBase<MultiCheckboxFormControl, MultiCheckboxFormControlBuilder>
    {
        public MultiCheckboxFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public MultiCheckboxFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var multiCheckboxFormControl = new MultiCheckboxFormControl() { Options = _optionsBuilder.Build() };
            return Build(componentInfo, multiCheckboxFormControl);
        }
    }
}
