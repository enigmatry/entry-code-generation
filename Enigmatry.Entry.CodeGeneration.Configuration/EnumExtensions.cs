using System;
using System.ComponentModel;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration
{
    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum o) => o.GetAttribute<TEnum, DescriptionAttribute>().Description;

        public static TDescriptionAttribute GetAttribute<TEnum, TDescriptionAttribute>(this TEnum o)
            where TDescriptionAttribute : DescriptionAttribute
        {
            TDescriptionAttribute? result = o.FindAttribute<TEnum, TDescriptionAttribute>();
            Type attributeType = typeof(TDescriptionAttribute);
            return result ?? throw new InvalidOperationException($"Attribute of type {attributeType} was not found");
        }

        private static TDescriptionAttribute? FindAttribute<TEnum, TDescriptionAttribute>(this TEnum o)
            where TDescriptionAttribute : DescriptionAttribute
        {
            Type enumType = o!.GetType();
            FieldInfo? field = enumType.GetField(o.ToString() ?? String.Empty);
            Type attributeType = typeof(TDescriptionAttribute);
            var attributes = field != null ? field.GetCustomAttributes(attributeType, false) : new object[0];
            return attributes.Length == 0 ? null : (TDescriptionAttribute)attributes[0];
        }
    }
}
