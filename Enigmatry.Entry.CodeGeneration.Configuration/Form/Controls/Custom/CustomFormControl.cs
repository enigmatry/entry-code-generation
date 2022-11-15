using System;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class CustomFormControl : FormControl
    {
        public string ControlTypeName { get; set; } = String.Empty;
        public override string FormlyType => ControlTypeName;
    }
}
