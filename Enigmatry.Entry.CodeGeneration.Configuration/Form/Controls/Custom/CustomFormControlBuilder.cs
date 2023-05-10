using System;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

/// <summary>
/// The CustomFormControlBuilder class provides a fluent API for configuring a custom form component.
/// </summary>
/// <remarks>
/// Use this builder to set the custom form component's custom control type,
/// as well as any other properties inherited from the base control builder.
/// </remarks>
public class CustomFormControlBuilder : BaseControlBuilder<CustomFormControl, CustomFormControlBuilder>
{
    private string _controlTypeName = String.Empty;

    public CustomFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public CustomFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    public CustomFormControlBuilder WithCustomControlType(string controlTypeName)
    {
        _controlTypeName = controlTypeName;
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var customFormControl = new CustomFormControl { ControlTypeName = _controlTypeName };
        return Build(componentInfo, customFormControl);
    }
}
