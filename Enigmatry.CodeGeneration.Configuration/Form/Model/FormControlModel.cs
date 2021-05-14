using System;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public class FormControlModel
    {
        public string Property { get; set; } = String.Empty;
        public string DisplayName { get; set; } = String.Empty;
        public bool IsVisible { get; set; }
        public bool IsReadonly { get; set; }
        public FormControlType Type { get; set; }
        public string Placeholder { get; set; } = String.Empty;
    }
}
