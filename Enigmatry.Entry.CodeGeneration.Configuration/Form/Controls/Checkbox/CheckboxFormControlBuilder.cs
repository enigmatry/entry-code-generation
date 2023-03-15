using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class CheckboxFormControlBuilder: BaseControlBuilder<CheckboxFormControl, CheckboxFormControlBuilder>
{
    public CheckboxFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public CheckboxFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var checkboxFormControl = new CheckboxFormControl();
        return Build(componentInfo, checkboxFormControl);
    }
}