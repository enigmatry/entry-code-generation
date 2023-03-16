namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class TextareaFormControl : InputControlBase
{
    public override string FormlyType => "textarea";
    public int Rows { get; set; }
    public int Cols { get; set; }
    public bool AutoResize { get; set; }
    public int AutoResizeMinRows { get; set; }
    public int AutoResizeMaxRows { get; set; }
}
