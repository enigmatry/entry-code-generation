using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class EmailFormControlBuilder : InputControlBuilderBase<EmailFormControl, EmailFormControlBuilder>
    {
        public EmailFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public EmailFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var emailFormControl = new EmailFormControl();
            return Build(componentInfo, emailFormControl);
        }
    }
}
