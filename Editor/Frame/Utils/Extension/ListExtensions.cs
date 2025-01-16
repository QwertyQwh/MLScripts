using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace QFramework.Extenstions
{
    public static class ListExtensions
    {
        public static bool IsEmpty<T>(this T[] target)
        {
            return (null == target || target.Length == 0);
        }

        public static bool IsEmpty<T>(this T[][] target)
        {
            return (null == target || target.Length == 0);
        }

        public static bool IsEmpty<T>(this List<T> target)
        {
            return (null == target || target.Count == 0);
        }

        public static bool IsEmpty<T>(this IList<T> target)
        {
            return (null == target || target.Count == 0);
        }

        public static bool IsEmpty<T, V>(this Dictionary<T, V> target)
        {
            return (null == target || target.Count == 0);
        }

        public static T DeepCopy<T>(this T obj)
        {
            object retval;
            using (var stream = new MemoryStream())
            {
                //var formatter = new XmlSerializer(typeof(T));
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                retval = formatter.Deserialize(stream);
                stream.Close();
            }
            return (T)retval;
        }

        public static void CurrentCultureSort(this List<string> list, bool ignoreCase = false)
        {
            if (ignoreCase)
            {
                list.Sort(CompareCurrentCultureIgnoreCase);
            }
            else
            {
                list.Sort(CompareCurrentCulture);
            }
        }

        private static int CompareCurrentCulture(string lhs, string rhs)
        {
            return string.Compare(lhs, rhs, StringComparison.CurrentCulture);
        }

        private static int CompareCurrentCultureIgnoreCase(string lhs, string rhs)
        {
            return string.Compare(lhs, rhs, StringComparison.CurrentCultureIgnoreCase);
        }

        public static void RemoveAll(this List<string> list, string[] array, bool ingore = false)
        {
            if (ingore)
            {
                list.RemoveAll(lhs => -1 != Array.FindIndex(array, rhs => 0 == CompareCurrentCultureIgnoreCase(lhs, rhs)));
            }
            else
            {
                list.RemoveAll(lhs => -1 != Array.FindIndex(array, rhs => 0 == CompareCurrentCulture(lhs, rhs)));
            }
        }
    }
}