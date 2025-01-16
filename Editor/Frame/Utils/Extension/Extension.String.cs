using System.Text;
using System.Text.RegularExpressions;

namespace Game.Runtime
{
    static partial class Extension
    {
        private static StringBuilder s_Builder = new StringBuilder();

        /// <summary>
        /// 字符串是否int类型
        /// </summary>
        /// <param name="content"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsNumberic(this string content, out int result)
        {
            result = -1;
            var regex = new Regex(@"^\d+$");
            if (regex.IsMatch(content))
            {
                result = int.Parse(content);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 字符串是否int类型
        /// </summary>
        /// <param name="content"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsNumberic(this string content, out long result)
        {
            result = -1;
            var regex = new Regex(@"^\d+$");
            if (regex.IsMatch(content))
            {
                result = long.Parse(content);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 字符串是否int类型
        /// </summary>
        /// <param name="content"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsNumberic(this string content, out ulong result)
        {
            result = ulong.MinValue;
            var regex = new Regex(@"^\d+$");
            if (regex.IsMatch(content))
            {
                result = ulong.Parse(content);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 字符串快速拼接
        /// </summary>
        /// <param name="first"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static string ToString(this string first, params object[] contents)
        {
            if (!string.IsNullOrEmpty(first))
            {
                s_Builder.Clear();
                s_Builder.Append(first);
                foreach (var content in contents)
                {
                    s_Builder.Append(content);
                }
                return s_Builder.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 字符串快速拼接
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static string ToFastString(params object[] contents)
        {
            if (contents.Length > 0)
            {
                s_Builder.Clear();
                foreach (var content in contents)
                {
                    s_Builder.Append(content);
                }
                return s_Builder.ToString();

            }
            return string.Empty;
        }
    }
}