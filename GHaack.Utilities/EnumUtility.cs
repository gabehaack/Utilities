using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GHaack.Utilities
{
    public static class EnumUtility
    {
        private static readonly DataGenerator _dataGenerator = new DataGenerator();

        /// <summary>
        /// Parse a string value corresponding to an enum name.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="enumName">String to parse.</param>
        /// <returns>A TEnum matching the string input, or the default TEnum value if the string does not match any TEnum value.</returns>
        public static TEnum Parse<TEnum>(string enumName) where TEnum : struct, IConvertible
        {
            TEnum result;
            Enum.TryParse(enumName, out result);
            return result;
        }

        /// <summary>
        /// Parse an integer value corresponding to an enum value.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="enumValue">Int to parse.</param>
        /// <returns>A TEnum matching the integer input, or the default TEnum value if the integer does not match any TEnum value.</returns>
        public static TEnum Parse<TEnum>(int enumValue) where TEnum : struct, IConvertible
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            return values.FirstOrDefault(v => v.Equals((TEnum)Enum.ToObject(typeof(TEnum), enumValue)));
        }

        /// <summary>
        /// Get a random value from an enum.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <returns>A random value from the enum type.</returns>
        public static TEnum GetRandomEnumValue<TEnum>() where TEnum : struct, IConvertible
        {
            var values = Enum.GetValues(typeof(TEnum));
            int length = values.Length;
            return (TEnum)values.GetValue(_dataGenerator.Index(length));
        }

        /// <summary>
        /// Get a random value from an enum excluding blacklist values.
        /// </summary>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="blackList">Blacklist values that will not be returned.</param>
        /// <returns>A random value from the enum type.</returns>
        public static TEnum GetRandomEnumValue<TEnum>(IEnumerable<TEnum> blackList) where TEnum : struct, IConvertible
        {
            var values = Enum.GetValues(typeof(TEnum));
            var enumValues = values.Cast<TEnum>().ToList();
            foreach (var blackListItem in blackList)
            {
                enumValues.Remove(blackListItem);
            }
            if (!enumValues.Any())
            {
                throw new Exception("No possible return values.");
            }
            return enumValues[_dataGenerator.Index(enumValues.Count)];
        }

        /// <summary>
        /// Get an attribute for this enum value.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to get.</typeparam>
        /// <param name="enumValue">This enum value.</param>
        /// <returns>The specified attribute for this enum value.</returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            var type = enumValue.GetType();
            var memberInfo = type.GetMember(enumValue.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(TAttribute), false);
            return (attributes.Length > 0) ? (TAttribute)attributes[0] : null;
        }

        /// <summary>
        /// Gets the Description attribute for this enum value.
        /// </summary>
        /// <param name="enumVal">The enum value.</param>
        /// <returns>The string value of the Description attribute, or an empty string if no Description attribute exists.</returns>
        public static string GetDescription(this Enum enumVal)
        {
            var descriptionAttribute = enumVal.GetAttribute<DescriptionAttribute>();
            return descriptionAttribute != null ? descriptionAttribute.Description : String.Empty;
        }
    }
}
