using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class CheckboxFormControlBuilder : BaseControlBuilder<CheckboxFormControl, CheckboxFormControlBuilder>
{
    protected bool _defaultValue;

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
        var checkboxFormControl = new CheckboxFormControl();
        checkboxFormControl.DefaultValue = _defaultValue;
        return Build(componentInfo, checkboxFormControl);
    }
}
