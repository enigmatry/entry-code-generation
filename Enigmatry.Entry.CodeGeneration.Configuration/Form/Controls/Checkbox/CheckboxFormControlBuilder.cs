using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

/// <summary>
/// The CheckboxFormControlBuilder class provides a fluent API for configuring a check-box form component.
/// </summary>
public class CheckboxFormControlBuilder: BaseControlBuilder<CheckboxFormControl, CheckboxFormControlBuilder>
{
    private bool? _defaultValue;

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
        _defaultValue = defaultValue;
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var checkboxFormControl = new CheckboxFormControl { DefaultValue = _defaultValue };
        return Build(componentInfo, checkboxFormControl);
    }
}
