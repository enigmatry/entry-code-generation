namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class SelectFormControl : SelectControlBase
{
    public override string ControlType => ControlTypes.Select;
    public string? DefaultValue { get; set; }
}
