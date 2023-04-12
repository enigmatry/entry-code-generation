namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class CheckboxFormControl : FormControl
{
    public override string FormlyType => FormlyTypes.CheckBox;
    public bool? DefaultValue { get; set; }
}
