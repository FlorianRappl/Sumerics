using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using OxyPlot;

namespace Sumerics.Controls
{
    public static class ExtensionMethods
    {
        public static bool IsCtrl(this KeyEventArgs keyEvent, Key value)
        {
            return keyEvent.KeyboardDevice.Modifiers == ModifierKeys.Control
                && keyEvent.Key == value;
        }

        public static bool IsModified(this KeyEventArgs keyEvent, Key value)
        {
            return keyEvent.KeyboardDevice.Modifiers != ModifierKeys.None
                && keyEvent.Key == value;
        }

        public static bool IsCtrlShift(this KeyEventArgs keyEvent, Key value)
        {
            return keyEvent.KeyboardDevice.Modifiers == ModifierKeys.Control
                && keyEvent.KeyboardDevice.Modifiers == ModifierKeys.Shift
                && keyEvent.Key == value;
        }

        public static bool Is(this KeyEventArgs keyEvent, Key value)
        {
            return keyEvent.KeyboardDevice.Modifiers == ModifierKeys.None && keyEvent.Key == value;
        }

        public static byte ToHexColor(this string str, int start)
        {
            var sub = str.Substring(start, 2);
            return byte.Parse(sub, NumberStyles.HexNumber);
        }

        public static Color FromHexToColor(this string str)
        {
            return Color.FromRgb(str.ToHexColor(1), str.ToHexColor(3), str.ToHexColor(5));
        }

        public static byte ToRgbColor(this string str)
        {
            var sub = Math.Min(int.Parse(str), 255);
            return Convert.ToByte(sub);
        }

        public static Color FromRgbToColor(this string str)
        {
            var numbers = "0123456789".ToCharArray();
            int start = 4;
            int currentIndex = 0;
            var colors = new byte[3];
            str = str.Replace(" ", "");
           
            for (var i = 5; i < str.Length; i++)
            {
                if(!numbers.Contains(str[i]))
                {
                    var color = str.Substring(start, i - start);
                    colors[currentIndex] = color.ToRgbColor();
                    currentIndex++;
                    start = i + 1;
                }

                if(currentIndex == 3)
                    break;
            }

            return Color.FromRgb(colors[0], colors[1], colors[2]);
		}

		public static OxyColor OxyColorFromString(this string color)
		{
			var c = ColorFromString(color);
			return OxyColor.FromArgb(c.A, c.R, c.G, c.B);
		}

		public static Brush BrushFromString(this string color)
		{
			return new SolidColorBrush(ColorFromString(color));
		}

		public static Color ColorFromString(this string color)
		{
			if (hexColorRegex.IsMatch(color))
				return color.FromHexToColor();

			if (rgbColorRegex.IsMatch(color))
				return color.FromRgbToColor();

			var props = typeof(Colors).GetProperties();

			foreach (var prop in props)
			{
				if (prop.Name.Equals(color, StringComparison.InvariantCultureIgnoreCase))
					return (Color)prop.GetValue(null, null);
			}

			return Colors.Black;
		}

		public static string ToHtml(this Color color)
		{
			return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}

		public static string ToHtml(this SolidColorBrush brush)
		{
			return brush.Color.ToHtml();
		}

		static readonly Regex hexColorRegex = new Regex(@"^\#[0-9A-Fa-f]{6}");
		static readonly Regex rgbColorRegex = new Regex(@"^rgb\(\s*[0-9]{1,3}\s*,\s*[0-9]{1,3}\s*,\s*[0-9]{1,3}\s*\)");
    }
}
