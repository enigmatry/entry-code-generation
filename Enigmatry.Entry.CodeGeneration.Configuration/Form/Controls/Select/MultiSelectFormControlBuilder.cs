using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class MultiSelectFormControlBuilder : SelectControlBuilderBase<MultiSelectFormControl, MultiSelectFormControlBuilder>
{
    public MultiSelectFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public MultiSelectFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var multiSelectFormControl = new MultiSelectFormControl { Options = _optionsBuilder.Build() };
        return Build(componentInfo, multiSelectFormControl);
    }
}