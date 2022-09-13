using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class InputFormControlBuilder : InputControlBuilderBase<InputFormControl, InputFormControlBuilder>
    {
        public InputFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public InputFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var inputFormControl = new InputFormControl();
            return Build(componentInfo, inputFormControl);
        }
    }
}
