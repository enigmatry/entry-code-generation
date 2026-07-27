namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class CheckboxFormControl : FormControl
{
    public override string ControlType => ControlTypes.CheckBox;
    public bool? DefaultValue { get; set; }
}
