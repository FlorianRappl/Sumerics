﻿namespace Sumerics.Controls
{
    using OxyPlot;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using System.Windows.Media;

    public static class ExtensionMethods
    {
        static readonly Regex hexColorRegex = new Regex(@"^\#[0-9A-Fa-f]{6}");
        static readonly Regex rgbColorRegex = new Regex(@"^rgb\(\s*[0-9]{1,3}\s*,\s*[0-9]{1,3}\s*,\s*[0-9]{1,3}\s*\)");

        public static Boolean IsCtrl(this KeyEventArgs keyEvent, Key value)
        {
            return keyEvent.KeyboardDevice.Modifiers == ModifierKeys.Control
                && keyEvent.Key == value;
        }

        public static Boolean IsModified(this KeyEventArgs keyEvent, Key value)
        {
            return keyEvent.KeyboardDevice.Modifiers != ModifierKeys.None
                && keyEvent.Key == value;
        }

        public static Boolean IsCtrlShift(this KeyEventArgs keyEvent, Key value)
        {
            return keyEvent.KeyboardDevice.Modifiers == ModifierKeys.Control
                && keyEvent.KeyboardDevice.Modifiers == ModifierKeys.Shift
                && keyEvent.Key == value;
        }

        public static Boolean Is(this KeyEventArgs keyEvent, Key value)
        {
            return keyEvent.KeyboardDevice.Modifiers == ModifierKeys.None && keyEvent.Key == value;
        }

        public static Byte ToHexColor(this String str, Int32 start)
        {
            var sub = str.Substring(start, 2);
            return Byte.Parse(sub, NumberStyles.HexNumber);
        }

        public static Color FromHexToColor(this String str)
        {
            return Color.FromRgb(str.ToHexColor(1), str.ToHexColor(3), str.ToHexColor(5));
        }

        public static Byte ToRgbColor(this String str)
        {
            var sub = Math.Min(Int32.Parse(str), 255);
            return Convert.ToByte(sub);
        }

        public static Color FromRgbToColor(this String str)
        {
            var numbers = "0123456789".ToCharArray();
            var start = 4;
            var currentIndex = 0;
            var colors = new Byte[3];
            str = str.Replace(" ", "");
           
            for (var i = 5; i < str.Length && currentIndex != 3; i++)
            {
                if (!numbers.Contains(str[i]))
                {
                    var color = str.Substring(start, i - start);
                    colors[currentIndex] = color.ToRgbColor();
                    currentIndex++;
                    start = i + 1;
                }
            }

            return Color.FromRgb(colors[0], colors[1], colors[2]);
		}

		public static OxyColor OxyColorFromString(this String color)
		{
			var c = ColorFromString(color);
			return OxyColor.FromArgb(c.A, c.R, c.G, c.B);
		}

		public static Brush BrushFromString(this String color)
		{
			return new SolidColorBrush(ColorFromString(color));
		}

		public static Color ColorFromString(this String color)
		{
            if (hexColorRegex.IsMatch(color))
            {
                return color.FromHexToColor();
            }

            if (rgbColorRegex.IsMatch(color))
            {
                return color.FromRgbToColor();
            }

			var props = typeof(Colors).GetProperties();

			foreach (var prop in props)
			{
                if (prop.Name.Equals(color, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (Color)prop.GetValue(null, null);
                }
			}

			return Colors.Black;
		}

		public static String ToHtml(this Color color)
		{
			return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}

		public static String ToHtml(this SolidColorBrush brush)
		{
			return brush.Color.ToHtml();
		}
    }
}
