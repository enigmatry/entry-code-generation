using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class RichTextInputFormControlBuilder : InputControlBuilderBase<RichTextInputFormControl, RichTextInputFormControlBuilder>
{
    private RichTextEditor _editor = RichTextEditor.Ckeditor;

    public RichTextInputFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    public RichTextInputFormControlBuilder(string propertyName) : base(propertyName)
    {
    }

    public RichTextInputFormControlBuilder WithEditor(RichTextEditor editor)
    {
        _editor = editor;
        return this;
    }

    public override FormControl Build(ComponentInfo componentInfo)
    {
        var richTextInputFormControl = new RichTextInputFormControl { Editor = _editor };
        return Build(componentInfo, richTextInputFormControl);
    }
}
