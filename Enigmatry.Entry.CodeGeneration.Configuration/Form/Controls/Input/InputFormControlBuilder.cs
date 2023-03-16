using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class InputFormControlBuilder : InputControlBuilderBase<InputFormControl, InputFormControlBuilder>
{
    public InputFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public InputFormControlBuilder(string propertyName) : base(propertyName)
    {
    }
}
