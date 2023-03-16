namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public abstract class InputControlBase : FormControl
{
    public override string FormlyType => "input";
    public bool ShouldAutocomplete { get; set; }
}
