/**
 * Author NBear - Nguyen Ba Hung - nbhung71711@gmail.com 
 **/

using UnityEngine;

namespace Utilities.Common
{
    public static class ColorExtension
    {
        private const float LightOffset = 0.0625f;
        private const float DarkerFactor = 0.9f;

        /// <summary>
        /// Returns the same color, but with the specified alpha.
        /// </summary>
        public static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Returns a new color that is this color inverted.
        /// </summary>
        public static Color Invert(this Color color)
        {
            return new Color(1 - color.r, 1 - color.g, 1 - color.b, color.a);
        }

        /// <summary>
		/// Returns an opaque version of the given color.
		/// </summary>
        public static Color Opaque(this Color color)
        {
            return new Color(color.r, color.g, color.b);
        }

        /// <summary>
        /// Returns whether the color is black or almost black.
        /// </summary>
        public static bool IsApproximatelyBlack(this Color color)
        {
            return color.r + color.g + color.b <= Mathf.Epsilon;
        }

        /// <summary>
        /// Returns whether the color is white or almost white.
        /// </summary>
        public static bool IsApproximatelyWhite(this Color color)
        {
            return color.r + color.g + color.b >= 1 - Mathf.Epsilon;
        }

        /// <summary>
        /// Returns a color lighter than the given color.
        /// </summary>
        public static Color Lighter(this Color color)
        {
            return new Color(
                color.r + LightOffset,
                color.g + LightOffset,
                color.b + LightOffset,
                color.a);
        }

        /// <summary>
        /// Returns a color darker than the given color.
        /// </summary>
        public static Color Darker(this Color color)
        {
            return new Color(
                color.r - LightOffset,
                color.g - LightOffset,
                color.b - LightOffset,
                color.a);
        }

        /// <summary>
        /// Returns the brightness of the color, 
        /// defined as the average off the three color channels.
        /// </summary>
        public static float Brightness(this Color color)
        {
            return (color.r + color.g + color.b) / 3;
        }

        public static string ToHex(this Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }
    }

    public static class ColorHelper
    {
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }
    }
}