namespace Sumerics.Controls
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

	public class CountConverter : IValueConverter
	{
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			return (Int32)value > 1;
		}

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
