using System;
using UnityEngine;

    public static class ColorExtensions
    {
        public static string ToHex(this Color color)
        {
            return string.Format(
                "{0}{1}{2}{3}",
                ToHex((int) (color.r * 255)),
                ToHex((int) (color.g * 255)),
                ToHex((int) (color.b * 255)),
                ToHex((int) (color.a * 255)));
        }

        private static string ToHex(int intVal)
        {
            return intVal.ToString("X2");
        }

        public static Color FromRgba(string color)
        {
            var val = Convert.ToUInt32(color, 16);
            var r = val >> 24 & 0xff;
            var g = val >> 16 & 0xff;
            var b = val >> 8 & 0xff;
            var a = val & 0xff;
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        public static Color RGBMultiplied(this Color color, float multiplier)
        {
            return new Color(color.r * multiplier, color.g * multiplier, color.b * multiplier, color.a);
        }

        public static Color AlphaMultiplied(this Color color, float multiplier)
        {
            return new Color(color.r, color.g, color.b, color.a * multiplier);
        }
    }
