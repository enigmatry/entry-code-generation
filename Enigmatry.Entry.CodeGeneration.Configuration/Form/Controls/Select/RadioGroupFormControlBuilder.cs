using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class RadioGroupFormControlBuilder : SelectControlBuilderBase<RadioGroupFormControl, RadioGroupFormControlBuilder>
{
    private string? _defaultValue;

    public RadioGroupFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public RadioGroupFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    /// <summary>
    /// Set control default value
    /// </summary>
    public RadioGroupFormControlBuilder WithDefaultValue(string defaultValue)
    {
        _defaultValue = defaultValue;
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var radioFormControl = new RadioGroupFormControl()
        {
            Options = _optionsBuilder.Build(),
            DefaultValue = _defaultValue
        };
        return Build(componentInfo, radioFormControl);
    }
}
