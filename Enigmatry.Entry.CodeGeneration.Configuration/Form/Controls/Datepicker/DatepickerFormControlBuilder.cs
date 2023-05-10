using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

/// <summary>
/// The DatepickerFormControlBuilder class provides a fluent API for configuring a date-picker form component.
/// </summary>
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
