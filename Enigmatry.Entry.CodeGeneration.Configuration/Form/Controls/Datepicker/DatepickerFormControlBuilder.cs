using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

/// <summary>
/// The DatepickerFormControlBuilder class provides a fluent API for configuring a date-picker form component.
/// </summary>
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
        var datepickerFormControl = new DatepickerFormControl { DefaultValue = _defaultValue };
        return Build(componentInfo, datepickerFormControl);
    }
}
