using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

/// <summary>
/// The InputControlBuilderBase class provides a fluent API for configuring an input form component.
/// </summary>
/// <remarks>
/// Use this builder to set the input form control's should auto-complete,
/// as well as any other properties inherited from the base control builder <see cref="BaseControlBuilder{TControl, TBuilder}"/>.
/// </remarks>
public class InputControlBuilderBase<T, TBuilder> : BaseControlBuilder<T, TBuilder>
    where T : InputControlBase, new()
    where TBuilder : InputControlBuilderBase<T, TBuilder>
{
    private bool? _shouldAutocomplete;

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

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var inputFormControl = new T { ShouldAutocomplete = _shouldAutocomplete };
        return Build(componentInfo, inputFormControl);
    }
}
