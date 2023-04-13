namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class SelectFormControl : SelectControlBase
{
    public override string FormlyType => FormlyTypes.Select;
    public string? DefaultValue { get; set; }
}
