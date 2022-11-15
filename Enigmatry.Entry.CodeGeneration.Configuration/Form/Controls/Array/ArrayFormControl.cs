using System;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls.Array
{
    public class ArrayFormControl: FormControl
    {
        public string? ControlTypeName { get; set; }
        public override string FormlyType => ControlTypeName ?? String.Empty;
        public FormControl FormControlGroup { get; set; } = new FormControlGroup();
    }
}
