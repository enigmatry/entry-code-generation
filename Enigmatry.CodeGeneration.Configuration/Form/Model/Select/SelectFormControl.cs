using System.Collections.Generic;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectFormControl : FormControl
    {
        public IEnumerable<SelectOption> FixedOptions { get; set; } = new List<SelectOption>();
        public string OptionValueKey { get; set; } = $"{nameof(SelectOption.Value).Camelize()}";
        public string OptionDisplayKey { get; set; } = $"{nameof(SelectOption.DisplayName).Camelize()}";
    }
}
