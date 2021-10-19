using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectFormControlBuilder
    {
        private readonly string _propertyName;
        private IEnumerable<SelectOption> _fixedValues = new List<SelectOption>();
        private string _valueKey = $"{nameof(SelectOption.Value).Camelize()}";
        private string _displayKey = $"{nameof(SelectOption.DisplayName).Camelize()}";

        public SelectFormControlBuilder(string propertyName)
        {
            _propertyName = propertyName;
        }

        public SelectFormControlBuilder WithFixedValues(IEnumerable<SelectOption> fixedValues)
        {
            _fixedValues = fixedValues.ToList();
            return this;
        }

        public SelectFormControlBuilder WithFixedValues<T>() where T : Enum
        {
            _fixedValues = Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Select(x => new SelectOption(Convert.ToInt32(x), GetDisplayName<T>(x.ToString())))
                .ToList();
            return this;
        }

        public SelectFormControlBuilder WithValueKey(string valueKey)
        {
            _valueKey = valueKey.Camelize();
            return this;
        }

        public SelectFormControlBuilder WithDisplayKey(string displayKey)
        {
            _displayKey = displayKey.Camelize();
            return this;
        }

        public SelectFormControl Build()
        {
            return new SelectFormControl { FixedOptions = _fixedValues, OptionValueKey = _valueKey, OptionDisplayKey = _displayKey, };
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
    }
}
