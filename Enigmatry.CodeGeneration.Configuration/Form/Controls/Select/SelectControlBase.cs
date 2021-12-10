namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public abstract class SelectControlBase : FormControl
    {
        public SelectOptions Options { get; set; } = new SelectOptions();
    }
}
