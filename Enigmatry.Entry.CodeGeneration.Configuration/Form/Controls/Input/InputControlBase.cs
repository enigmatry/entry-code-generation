namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public abstract class InputControlBase : FormControl
{
    public override string FormlyType => FormlyTypes.Input;
    public bool? ShouldAutocomplete { get; set; }
    public string? DefaultValue { get; set; }
}
