using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class SelectOptionsBuilder
    {
        private IEnumerable<SelectOption> _fixedValues = new List<SelectOption>();
        private string _valueKey = String.Empty;
        private string _displayKey = String.Empty;
        private string _sortKey = String.Empty;
        private bool _hasDynamicValues;
        private SelectOption? _emptyOption;
        private SelectOption? _selectAllOption;

        public SelectOptionsBuilder WithFixedValues(IEnumerable<SelectOption> fixedValues)
        {
            _fixedValues = fixedValues.ToList();
            return this;
        }

        public SelectOptionsBuilder WithFixedValues<T>() where T : Enum
        {
            _fixedValues = Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Select(x => new SelectOption(Convert.ToInt32(x), GetDisplayName<T>(x.ToString()), GetTranslationId(x)))
                .ToList();
            return this;
        }

        public SelectOptionsBuilder WithValueKey(string valueKey)
        {
            _valueKey = valueKey.Camelize();
            return this;
        }

        public SelectOptionsBuilder WithDisplayKey(string displayKey)
        {
            _displayKey = displayKey.Camelize();
            return this;
        }

        public SelectOptionsBuilder WithSortKey(string sortKey)
        {
            _sortKey = sortKey.Camelize();
            return this;
        }

        public SelectOptionsBuilder WithDynamicValues()
        {
            _hasDynamicValues = true;
            return this;
        }

        public SelectOptionsBuilder WithEmptyOption(string label, string? translationId = null)
        {
            _emptyOption = new SelectOption(null, label, translationId ?? $"empty-option.{label.Kebaberize()}");
            return this;
        }

        public SelectOptionsBuilder WithSelectAllOption(string label, string? translationId = null)
        {
            _selectAllOption = new SelectOption(null, label, translationId ?? $"select-all-option.{label.Kebaberize()}");
            return this;
        }

        public SelectOptions Build()
        {
            return new SelectOptions
            {
                FixedOptions = GetFixedValuesWithEmptyOption(),
                OptionValueKey = _valueKey,
                OptionDisplayKey = _displayKey,
                OptionSortKey = _sortKey,
                HasDynamicValues = _hasDynamicValues,
                EmptyOption = _emptyOption,
                SelectAllOption = _selectAllOption
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
