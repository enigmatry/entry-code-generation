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

    // Hide inherited method so it cannot be used with checkbox form control.
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
    private new CheckboxFormControlBuilder WithDefaultValue(string defaultValue) { return this; }
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0051 // Remove unused private members
}
