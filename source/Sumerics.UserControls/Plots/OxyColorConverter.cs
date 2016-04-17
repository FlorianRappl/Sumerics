namespace Sumerics.Controls.Plots
{
    using OxyPlot;
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class OxyColorConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var color = (OxyColor)value;
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Int32ColorConverter can only be used OneWay.");
        }
    }
}
