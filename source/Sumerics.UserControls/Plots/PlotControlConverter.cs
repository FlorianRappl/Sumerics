namespace Sumerics.Controls.Plots
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class PlotControlConverter : IValueConverter
    {
        static readonly ControlFactory Factory = new ControlFactory();

        public static FrameworkElement Default
        {
            get { return Factory.CreateDefault(); }
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value != null ? Factory.Create(value) : Default;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("PlotControlConverter can only be used OneWay.");
        }
    }
}
