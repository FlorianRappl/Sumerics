namespace Sumerics.Controls.Plots
{
    using Sumerics.Plots.Models;
    using Sumerics.Resources;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public sealed class ControlFactory : TypeFactory<Object, FrameworkElement>
    {
        public ControlFactory()
            : base(true)
        {
            Register<OxyPlotModel>(model => new Oxy2dPlotControl { DataContext = model });
            Register<GridPlotModel>(model => new GridPlotControl { DataContext = model });
            Register<WpfPlotModel>(model => new Wpf3dPlotControl { DataContext = model });
        }

        public override FrameworkElement CreateDefault()
        {
            return new TextBlock
            {
                Text = Messages.PlotPlaceholder,
                FontSize = 16,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Colors.DarkGray)
            };
        }
    }
}
