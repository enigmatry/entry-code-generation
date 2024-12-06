using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;

public class ArrayFormControlBuilder<T> : BaseControlBuilder<ArrayFormControl, ArrayFormControlBuilder<T>>
{
    private string? _controlTypeName = String.Empty;
    private readonly FormControlGroupBuilder<T> _formControlGroupBuilder = new FormControlGroupBuilder<T>(String.Empty);

    public ArrayFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public ArrayFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    public ArrayFormControlBuilder<T> WithCustomControlType(string controlTypeName)
    {
        _controlTypeName = controlTypeName;
        return this;
    }

    public ArrayFormControlBuilder<T> WithItemConfiguration(Action<FormControlGroupBuilder<T>> configure)
    {
        configure(_formControlGroupBuilder);
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var arrayFormControl = new ArrayFormControl
        {
            ControlTypeName = _controlTypeName,
            FormControlGroup = _formControlGroupBuilder.Build(componentInfo)
        };
        return Build(componentInfo, arrayFormControl);
    }
}