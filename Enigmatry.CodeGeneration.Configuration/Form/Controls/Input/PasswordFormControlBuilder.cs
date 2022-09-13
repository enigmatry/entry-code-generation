using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class PasswordFormControlBuilder : InputControlBuilderBase<PasswordFormControl, PasswordFormControlBuilder>
    {
        public PasswordFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public PasswordFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var passwordFormControl = new PasswordFormControl();
            return Build(componentInfo, passwordFormControl);
        }
    }
}
