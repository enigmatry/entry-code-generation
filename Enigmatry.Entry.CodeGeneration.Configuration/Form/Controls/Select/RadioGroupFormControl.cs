namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class RadioGroupFormControl : SelectControlBase
{
    public override string ControlType => ControlTypes.Radio;
    public string? DefaultValue { get; set; }
}
