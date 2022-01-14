﻿namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class ButtonFormControl : FormControl
    {
        public I18NString Text { get; set; } = I18NString.Empty;
        public string? ControlTypeName { get; set; }
        public override string FormlyType => ControlTypeName ?? "button";
    }
}
