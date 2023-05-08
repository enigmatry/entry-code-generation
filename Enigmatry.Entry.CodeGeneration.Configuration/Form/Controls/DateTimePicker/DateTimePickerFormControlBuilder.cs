using System;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class DateTimePickerFormControlBuilder :
    BaseControlBuilder<DateTimePickerFormControl, DateTimePickerFormControlBuilder>
{
    private DateTimeOffset? _defaultValue;

    public DateTimePickerFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public DateTimePickerFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    public DateTimePickerFormControlBuilder WithDefaultValue(DateTimeOffset defaultValue)
    {
        _defaultValue = defaultValue;
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var dateTimePickerFormControl = new DateTimePickerFormControl { DefaultValue = _defaultValue };
        return Build(componentInfo, dateTimePickerFormControl);
    }
}
