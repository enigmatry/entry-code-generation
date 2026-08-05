namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;

public class ArrayFormControl: FormControl
{
    public string? ControlTypeName { get; set; }
    public override string ControlType => ControlTypeName ?? String.Empty;
    public FormControl FormControlGroup { get; set; } = new FormControlGroup();
    public I18NString AddButtonLabel { get; set; } = I18NString.Empty;
    public I18NString RemoveButtonLabel { get; set; } = I18NString.Empty;
}