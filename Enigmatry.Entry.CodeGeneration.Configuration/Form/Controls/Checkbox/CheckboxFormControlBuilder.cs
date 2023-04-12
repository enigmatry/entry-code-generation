using System;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class CheckboxFormControlBuilder: BaseControlBuilder<CheckboxFormControl, CheckboxFormControlBuilder>
{
    public CheckboxFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public CheckboxFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    /// <summary>
    /// Set control default value
    /// </summary>
    public CheckboxFormControlBuilder WithDefaultValue(bool defaultValue)
    {
        _defaultValue = defaultValue ? "true" : "false";
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var checkboxFormControl = new CheckboxFormControl();
        return Build(componentInfo, checkboxFormControl);
    }

    // This method is deliberately made private so that users cannot set string default values!
#pragma warning disable IDE0051 // Remove unused private members
    private new CheckboxFormControlBuilder WithDefaultValue(string defaultValue)
    {
        throw new NotImplementedException($"String default values ({defaultValue}) are not supported in CheckBox from component!");
    }
#pragma warning restore IDE0051 // Remove unused private members
}
