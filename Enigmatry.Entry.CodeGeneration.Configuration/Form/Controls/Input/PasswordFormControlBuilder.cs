using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class PasswordFormControlBuilder : InputControlBuilderBase<PasswordFormControl, PasswordFormControlBuilder>
{
    public PasswordFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public PasswordFormControlBuilder(string propertyName) : base(propertyName)
    {
    }
}
