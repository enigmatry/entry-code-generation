using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectControlOptions
    {
        public IEnumerable<SelectOption> FixedOptions { get; set; } = new List<SelectOption>();
        public string OptionValueKey { get; set; } = $"{nameof(SelectOption.Value).Camelize()}";
        public string OptionDisplayKey { get; set; } = $"{nameof(SelectOption.DisplayName).Camelize()}";
        public SelectOption? EmptyOption { get; set; }
        public bool HasFixedValues => FixedOptions.Any();
        public bool HasDynamicValues { get; set; }
    }
}
