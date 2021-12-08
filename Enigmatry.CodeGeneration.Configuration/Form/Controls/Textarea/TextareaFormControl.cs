namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class TextareaFormControl : FormControl
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public override string FormlyType => "textarea";
    }
}
