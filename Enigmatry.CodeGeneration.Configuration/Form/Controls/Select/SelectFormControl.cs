namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class SelectFormControl : FormControl
    {
        public override string FormlyType => "select";
        public SelectOptions Options { get; set; } = new SelectOptions();
    }
}
