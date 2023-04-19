namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class RichTextInputFormControl : InputControlBase
{
    public RichTextEditor Editor { get; init; } = RichTextEditor.Ckeditor;
    public override string FormlyType => Editor.ToString().ToLowerInvariant();
}
