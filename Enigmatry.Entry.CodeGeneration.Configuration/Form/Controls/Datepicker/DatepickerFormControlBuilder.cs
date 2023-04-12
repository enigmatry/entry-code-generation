using System;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class DatepickerFormControlBuilder: BaseControlBuilder<DatepickerFormControl, DatepickerFormControlBuilder>
{
    private DateTimeOffset? _defaultValue;

    public DatepickerFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public DatepickerFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    /// <summary>
    /// Set control default value
    /// </summary>
    public DatepickerFormControlBuilder WithDefaultValue(DateTimeOffset defaultValue)
    {
        _defaultValue = defaultValue;
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var datepickerFormControl = new DatepickerFormControl();
        datepickerFormControl.DefaultValue = _defaultValue;
        return Build(componentInfo, datepickerFormControl);
    }
}
