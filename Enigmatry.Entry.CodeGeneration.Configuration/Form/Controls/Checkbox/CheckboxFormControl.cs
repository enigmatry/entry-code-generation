namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class CheckboxFormControl : FormControl
{
    public bool DefaultValue { get; set; }

    public override string FormlyType => "checkbox";

    public override string? DefaultValueAsString() => DefaultValue ? "true" : "false";
}
