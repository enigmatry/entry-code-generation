using System.Reflection;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;

public class ArrayFormControlBuilder<T> : BaseControlBuilder<ArrayFormControl, ArrayFormControlBuilder<T>>
{
    private string? _controlTypeName = String.Empty;
    private readonly FormControlGroupBuilder<T> _formControlGroupBuilder = new FormControlGroupBuilder<T>(String.Empty);
    private string? _addButtonLabel;
    private string? _addButtonTranslationId;
    private string? _removeButtonLabel;
    private string? _removeButtonTranslationId;

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

    /// <summary>
    /// Configures the label of the generated "add item" button (signals templates only).
    /// </summary>
    public ArrayFormControlBuilder<T> WithAddButtonLabel(string label, string? translationId = null)
    {
        _addButtonLabel = label;
        _addButtonTranslationId = translationId;
        return this;
    }

    /// <summary>
    /// Configures the label of the generated "remove item" button (signals templates only).
    /// </summary>
    public ArrayFormControlBuilder<T> WithRemoveButtonLabel(string label, string? translationId = null)
    {
        _removeButtonLabel = label;
        _removeButtonTranslationId = translationId;
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var translationIdPrefix = $"{componentInfo.TranslationId}.{_propertyName.Kebaberize()}";
        var arrayFormControl = new ArrayFormControl
        {
            ControlTypeName = _controlTypeName,
            FormControlGroup = _formControlGroupBuilder.Build(componentInfo),
            AddButtonLabel = new I18NString(_addButtonTranslationId ?? $"{translationIdPrefix}.add-item", _addButtonLabel ?? "Add"),
            RemoveButtonLabel = new I18NString(_removeButtonTranslationId ?? $"{translationIdPrefix}.remove-item", _removeButtonLabel ?? "Remove")
        };
        return Build(componentInfo, arrayFormControl);
    }
}