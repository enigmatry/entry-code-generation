namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public abstract class InputControlBase : FormControl
    {
        public override string FormlyType => "input";
        public abstract string? Type { get; set; }
    }
}
