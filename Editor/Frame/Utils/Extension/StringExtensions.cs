namespace QFramework.Extenstions
{
    public static class HelperExtensions
    {
        public static bool FastEqual(this string str, string other)
        {
            return string.CompareOrdinal(str, other) == 0;
        }
    }
}