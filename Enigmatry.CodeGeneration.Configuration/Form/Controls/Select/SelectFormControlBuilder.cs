using System;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class SelectFormControlBuilder : BaseControlBuilder<SelectFormControl, SelectFormControlBuilder>
    {
        private readonly SelectOptionsBuilder _optionsBuilder = new SelectOptionsBuilder();

        public SelectFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public SelectFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public SelectFormControlBuilder WithOptions(Action<SelectOptionsBuilder>? options = null)
        {
            options?.Invoke(_optionsBuilder);
            return this;
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var selectFormControl = new SelectFormControl { Options = _optionsBuilder.Build() };
            return Build(componentInfo, selectFormControl);
        }
    }
}
