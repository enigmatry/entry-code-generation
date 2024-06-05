using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class NumberFormControlBuilder : InputControlBuilderBase<NumberFormControl, NumberFormControlBuilder>
{
    public NumberFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public NumberFormControlBuilder(string propertyName) : base(propertyName)
    {
    }
}
