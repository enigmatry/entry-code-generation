namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class RadioGroupFormControl : SelectControlBase
{
    public override string FormlyType => FormlyTypes.Radio;
    public string? DefaultValue { get; set; }
}
