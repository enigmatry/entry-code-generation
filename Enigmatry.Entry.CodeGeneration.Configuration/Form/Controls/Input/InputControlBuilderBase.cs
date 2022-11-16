using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls
{
    public class InputControlBuilderBase<T> : BaseControlBuilder<T, InputControlBuilderBase<T>> where T : InputControlBase, new()
    {
        public InputControlBuilderBase(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public InputControlBuilderBase(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var inputFormControl = new T();
            return Build(componentInfo, inputFormControl);
        }
    }
}
