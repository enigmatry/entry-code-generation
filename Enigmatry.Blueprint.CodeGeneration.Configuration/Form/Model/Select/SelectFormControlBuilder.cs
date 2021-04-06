using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Enigmatry.Blueprint.CodeGeneration.Configuration.Services;
using Humanizer;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectFormControlBuilder
    {
        private PropertyInfo _propertyInfo;
        public SelectFormControlType SelectType { get; private set; }
        public LookupMethodBase LookupMethod { get; private set; } = null!;

        public SelectFormControlBuilder(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public SelectFormControlBuilder WithFixedValues(IEnumerable<SelectOption> fixedValues)
        {
            SelectType = SelectFormControlType.FixedSelect;
            LookupMethod = new FixedLookupMethod($"get{_propertyInfo.Name.Pascalize()}", fixedValues);
            return this;
        }

        public SelectFormControlBuilder WithFixedValues<T>() where T : Enum
        {
            SelectType = SelectFormControlType.FixedSelect;
            var fixedValues = Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Select(x => new SelectOption(Convert.ToInt32(x), GetDisplayName<T>(x.ToString())));
            LookupMethod = new FixedLookupMethod($"get{_propertyInfo.Name.Pascalize()}", fixedValues);
            return this;
        }

        public SelectFormControlBuilder WithCallbackMethod(MethodInfo? lookupMethodInfo)
        {
            if (lookupMethodInfo == null)
            {
                throw new InvalidEnumArgumentException("Missing async select callback method information.");
            }
            SelectType = SelectFormControlType.AsyncSelect;
            LookupMethod = new AsyncLookupMethod(lookupMethodInfo);
            return this;
        }

        public SelectFormControlBuilder DependsOn(FormControlBuilder leadingControlBuilder)
        {
            Check.IsAsyncSelectFormControl(leadingControlBuilder);

            LookupMethod
                .ArgumentNames.Add(leadingControlBuilder.PropertyInfo.Name);

            leadingControlBuilder
                .SelectControlBilder
                .LookupMethod
                .DependantMethods.Add(LookupMethod);

            return this;
        }

        public SelectFormControlModel Build()
        {
            return new SelectFormControlModel
            {
                SelectType = SelectType,
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
