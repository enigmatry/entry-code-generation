using Enigmatry.CodeGeneration.Configuration.Form.Controls.Datepicker;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class DatepickerFormControlBuilder: BaseControlBuilder<DatepickerFormControl, DatepickerFormControlBuilder>
    {
        public DatepickerFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public DatepickerFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var datepickerFormControl = new DatepickerFormControl();
            return Build(componentInfo, datepickerFormControl);
        }
    }
}
