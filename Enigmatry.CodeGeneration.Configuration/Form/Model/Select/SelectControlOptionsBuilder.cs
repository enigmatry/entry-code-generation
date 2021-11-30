using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectControlOptionsBuilder
    {
        private IEnumerable<SelectOption> _fixedValues = new List<SelectOption>();
        private string _valueKey = String.Empty;
        private string _displayKey = String.Empty;
        private bool _hasDynamicValues;
        private SelectOption? _emptyOption;

        public SelectControlOptionsBuilder WithFixedValues(IEnumerable<SelectOption> fixedValues)
        {
            _fixedValues = fixedValues.ToList();
            return this;
        }

        public SelectControlOptionsBuilder WithFixedValues<T>() where T : Enum
        {
            _fixedValues = Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Select(x => new SelectOption(Convert.ToInt32(x), GetDisplayName<T>(x.ToString()), GetTranslationId(x)))
                .ToList();
            return this;
        }

        public SelectControlOptionsBuilder WithValueKey(string valueKey)
        {
            _valueKey = valueKey.Camelize();
            return this;
        }

        public SelectControlOptionsBuilder WithDisplayKey(string displayKey)
        {
            _displayKey = displayKey.Camelize();
            return this;
        }

        public SelectControlOptionsBuilder WithDynamicValues()
        {
            _hasDynamicValues = true;
            return this;
        }

        public SelectControlOptionsBuilder WithEmptyOption(string label, string? transaltionId = null)
        {
            _emptyOption = new SelectOption(null!, label, transaltionId ?? $"empty-option.{label.Kebaberize()}");
            return this;
        }

        public SelectControlOptions Build()
        {
            return new SelectControlOptions
            {
                FixedOptions = GetFixedValuesWithEmptyOption(),
                OptionValueKey = _valueKey,
                OptionDisplayKey = _displayKey,
                HasDynamicValues = _hasDynamicValues,
                EmptyOption = _emptyOption
            };
        }

        private string GetDisplayName<T>(string value) where T : Enum
        {
            Type type = typeof(T);
            var name = Enum
                .GetNames(type)
                .FirstOrDefault(enumValueName => enumValueName.Equals(value, StringComparison.CurrentCultureIgnoreCase));

            if (name == null) { return String.Empty; }

            var field = type.GetField(name);
            var customAttribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return customAttribute?.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }

        private static string GetTranslationId<T>(T @enum) where T : Enum
        {
            return $"enum.{typeof(T).Name}.{@enum}".Kebaberize();
        }

        private IEnumerable<SelectOption> GetFixedValuesWithEmptyOption()
        {
            return _emptyOption != null
                ? (new[] { _emptyOption }).Concat(_fixedValues)
                : _fixedValues;
        }
    }
}
