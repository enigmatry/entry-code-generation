using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Enigmatry.CodeGeneration.Configuration.Services;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectFormControlBuilder
    {
        private readonly string _propertyName;
        public LookupMethodBase LookupMethod { get; private set; } = null!;

        public SelectFormControlBuilder(string propertyName)
        {
            _propertyName = propertyName;
            LookupMethod = new CustomLookupMethod($"get{propertyName.Pascalize()}");
        }

        public SelectFormControlBuilder WithFixedValues(IEnumerable<SelectOption> fixedValues)
        {
            LookupMethod = new FixedValuesLookupMethod($"get{_propertyName.Pascalize()}", fixedValues);
            return this;
        }

        public SelectFormControlBuilder WithFixedValues<T>() where T : Enum
        {
            var fixedValues = Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Select(x => new SelectOption(Convert.ToInt32(x), GetDisplayName<T>(x.ToString())));
            LookupMethod = new FixedValuesLookupMethod($"get{_propertyName.Pascalize()}", fixedValues);
            return this;
        }

        public SelectFormControlBuilder WithCallbackMethod(MethodInfo? lookupMethodInfo)
        {
            if (lookupMethodInfo == null)
            {
                throw new InvalidEnumArgumentException("Missing async select callback method information.");
            }
            LookupMethod = new AsyncLookupMethod(lookupMethodInfo);
            return this;
        }

        public SelectFormControl Build()
        {
            return new SelectFormControl
            {
                LookupMethod = LookupMethod
            };
        }

        public string GetDisplayName<T>(string value) where T : Enum
        {
            Type type = typeof(T);
            var name = Enum
                .GetNames(type)
                .FirstOrDefault(enumValueName => enumValueName.Equals(value, StringComparison.CurrentCultureIgnoreCase));

            if (name == null)
            {
                return String.Empty;
            }

            var field = type.GetField(name);
            var customAttribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return customAttribute?.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }
    }
}
