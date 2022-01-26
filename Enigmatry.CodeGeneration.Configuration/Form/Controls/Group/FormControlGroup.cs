﻿using Enigmatry.BuildingBlocks.Validation.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class FormControlGroup : FormControl
    {
        public string? WrapperElement { get; set; }
        public IList<FormControl> FormControls { get; set; } = new List<FormControl>();
        public override string FormlyType => WrapperElement ?? String.Empty;

        public override void ApplyValidationConfiguration(IEnumerable<IFormlyValidationRule> validationRules)
        {
            foreach (var formControl in FormControls)
            {
                formControl.ApplyValidationConfiguration(validationRules);
            }
        }

        public IEnumerable<TControl> FormControlsOfType<TControl>() where TControl : FormControl
        {
            return FormControls.OfType<TControl>().Concat(
                FormControls.OfType<FormControlGroup>()
                    .SelectMany(x => x.FormControlsOfType<TControl>()));
        }
    }
}
