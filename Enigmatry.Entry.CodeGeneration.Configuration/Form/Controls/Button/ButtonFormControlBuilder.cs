using System.Reflection;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class ButtonFormControlBuilder: BaseControlBuilder<ButtonFormControl, ButtonFormControlBuilder>
{
    private string? _text;
    private string? _textTranslationId;
    private string? _controlTypeName;

    public ButtonFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public ButtonFormControlBuilder(string name) : base(name)
    {
    }

    public ButtonFormControlBuilder WithText(string text)
    {
        _text = text;
        return this;
    }

    public ButtonFormControlBuilder WithTextTranslationId(string translationId)
    {
        _textTranslationId = translationId;
        return this;
    }

    public ButtonFormControlBuilder WithCustomControlType(string controlTypeName)
    {
        _controlTypeName = controlTypeName;
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var translationId = $"{componentInfo.TranslationId}.{_propertyName.Kebaberize()}.";

        var textTranslationId = _textTranslationId ?? $"{translationId}text";
        var text = _text ?? _propertyName.Humanize();

        var buttonFormControl = new ButtonFormControl { ControlTypeName = _controlTypeName, Text = new I18NString(textTranslationId, text) };
        return Build(componentInfo, buttonFormControl);
    }
}