using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class SelectFormControlBuilder : SelectControlBuilderBase<SelectFormControl, SelectFormControlBuilder>
{
    private string? _defaultValue;

    public SelectFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public SelectFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    /// <summary>
    /// Set control default value
    /// </summary>
    public SelectFormControlBuilder WithDefaultValue(string defaultValue)
    {
        _defaultValue = defaultValue;
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var selectFormControl = new SelectFormControl
        {
            Options = _optionsBuilder.Build(),
            DefaultValue = _defaultValue
        };
        return Build(componentInfo, selectFormControl);
    }
}
