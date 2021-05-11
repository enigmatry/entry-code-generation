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
        private PropertyInfo _propertyInfo;
        public LookupMethodBase LookupMethod { get; private set; } = null!;

        public SelectFormControlBuilder(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
            LookupMethod = new CustomLookupMethod($"get{_propertyInfo.Name.Pascalize()}");
        }

        public SelectFormControlBuilder WithFixedValues(IEnumerable<SelectOption> fixedValues)
        {
            LookupMethod = new FixedValuesLookupMethod($"get{_propertyInfo.Name.Pascalize()}", fixedValues);
            return this;
        }

        public SelectFormControlBuilder WithFixedValues<T>() where T : Enum
        {
            var fixedValues = Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Select(x => new SelectOption(Convert.ToInt32(x), GetDisplayName<T>(x.ToString())));
            LookupMethod = new FixedValuesLookupMethod($"get{_propertyInfo.Name.Pascalize()}", fixedValues);
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

        public SelectFormControlBuilder DependsOn(FormControlBuilder leadingFormControl)
        {
            Check.IsSelectFormControl(leadingFormControl);

            LookupMethod
                .ArgumentNames.Add(leadingFormControl.PropertyInfo.Name);

            leadingFormControl
                .Select
                .LookupMethod
                .DependentMethods.Add(LookupMethod);

            return this;
        }

        public SelectFormControlModel Build()
        {
            return new SelectFormControlModel
            {
                LookupMedhod = LookupMethod
            };
        }

        public string GetDisplayName<T>(string value) where T : Enum
        {
            Type type = typeof(T);
            var name = Enum
                .GetNames(type)
                .Where(enumValueName => enumValueName.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

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
