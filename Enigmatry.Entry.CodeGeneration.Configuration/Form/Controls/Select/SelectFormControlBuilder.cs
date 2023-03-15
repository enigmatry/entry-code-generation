using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class SelectFormControlBuilder : SelectControlBuilderBase<SelectFormControl, SelectFormControlBuilder>
{
    public SelectFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public SelectFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var selectFormControl = new SelectFormControl { Options = _optionsBuilder.Build() };
        return Build(componentInfo, selectFormControl);
    }
}