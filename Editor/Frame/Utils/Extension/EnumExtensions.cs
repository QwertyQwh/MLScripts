using System;
using System.ComponentModel;

namespace QFramework.Extenstions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var descriptions = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptions.Length == 0)
                return null;
            return (descriptions[0] as DescriptionAttribute).Description;
        }
    }
}