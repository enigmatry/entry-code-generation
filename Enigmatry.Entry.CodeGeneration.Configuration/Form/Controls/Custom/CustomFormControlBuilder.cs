using System;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class CustomFormControlBuilder : BaseControlBuilder<CustomFormControl, CustomFormControlBuilder>
    {
        private string _controlTypeName = String.Empty;

        public CustomFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public CustomFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public CustomFormControlBuilder WithCustomControlType(string controlTypeName)
        {
            _controlTypeName = controlTypeName;
            return this;
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var customFormControl = new CustomFormControl { ControlTypeName = _controlTypeName };
            return Build(componentInfo, customFormControl);
        }
    }
}
