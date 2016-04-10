namespace Sumerics
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

	public class IsNullConverter : IValueConverter
	{
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			return value == null;
		}

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
		}
	}
}
