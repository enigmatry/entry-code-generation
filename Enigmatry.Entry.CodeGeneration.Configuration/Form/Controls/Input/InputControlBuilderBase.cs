using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class InputControlBuilderBase<T, TBuilder> : BaseControlBuilder<T, TBuilder>
    where T : InputControlBase, new()
    where TBuilder : InputControlBuilderBase<T, TBuilder>
{
    protected bool? _shouldAutocomplete;
    protected string? _defaultValue;

    protected InputControlBuilderBase(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    protected InputControlBuilderBase(string propertyName) : base(propertyName)
    {
    }

    /// <summary>
    /// Configure form control autocomplete
    /// </summary>
    /// <param name="shouldAutocomplete"></param>
    /// <returns></returns>
    public TBuilder ShouldAutocomplete(bool shouldAutocomplete)
    {
        _shouldAutocomplete = shouldAutocomplete;
        return (TBuilder)this;
    }

    /// <summary>
    /// Set control default value
    /// </summary>
    public TBuilder WithDefaultValue(string defaultValue)
    {
        _defaultValue = defaultValue;
        return (TBuilder)this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var inputFormControl = new T
        {
            ShouldAutocomplete = _shouldAutocomplete,
            DefaultValue = _defaultValue
        };
        return Build(componentInfo, inputFormControl);
    }
}
