using System;
using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public class FormControlGroup : FormControl
    {
        public string? SectionWrapperElement { get; set; }
        public IList<FormControl> FormControls { get; set; } = new List<FormControl>();

        public override string GetFormlyType()
        {
            return SectionWrapperElement ?? String.Empty;
        }
    }
}
