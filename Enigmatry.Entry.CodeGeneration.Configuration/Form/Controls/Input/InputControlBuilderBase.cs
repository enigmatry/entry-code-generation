using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class InputControlBuilderBase<T, TBuilder> : BaseControlBuilder<T, TBuilder>
    where T : InputControlBase, new()
    where TBuilder : InputControlBuilderBase<T, TBuilder>
{
    protected bool _shouldAutocomplete = true;

    public InputControlBuilderBase(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public InputControlBuilderBase(string propertyName) : base(propertyName)
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

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var inputFormControl = new T();
        inputFormControl.ShouldAutocomplete = _shouldAutocomplete;
        return Build(componentInfo, inputFormControl);
    }
}
