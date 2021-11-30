﻿using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectControlOptions
    {
        public IEnumerable<SelectOption> FixedOptions { get; set; } = new List<SelectOption>();
        public string OptionValueKey { get; set; } = String.Empty;
        public string OptionDisplayKey { get; set; } = String.Empty;  
        public SelectOption? EmptyOption { get; set; }
        public bool HasDynamicValues { get; set; }
        public bool HasFixedValues => FixedOptions.Any();
        public bool HasCustomValueAndDisplayKeys => OptionValueKey.HasContent() && OptionDisplayKey.HasContent();
        public string DefaultOptionsAsString => HasFixedValues
            ? $"{{ valueProperty: '{nameof(SelectOption.Value).Camelize()}', labelProperty: '{nameof(SelectOption.DisplayName).Camelize()}' }}"
            : "{}";
    }
}
