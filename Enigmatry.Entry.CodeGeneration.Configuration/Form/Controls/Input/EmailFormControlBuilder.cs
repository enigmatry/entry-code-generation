using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class EmailFormControlBuilder : InputControlBuilderBase<EmailFormControl, EmailFormControlBuilder>
{
    public EmailFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public EmailFormControlBuilder(string propertyName) : base(propertyName)
    {
    }
}
