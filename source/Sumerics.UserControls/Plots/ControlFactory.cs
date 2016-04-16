namespace Sumerics.Controls.Plots
{
    using OxyPlot.Wpf;
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
            Register<OxyPlotModel>(model => new OxyPlotControl { DataContext = Unbind(model) });
            Register<GridPlotModel>(model => new GridPlotControl { DataContext = Unbind(model) });
            Register<WpfPlotModel>(model => new Wpf3dPlotControl { DataContext = Unbind(model) });
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

        static Object Unbind(WpfPlotModel model)
        {
            return model;
        }

        static Object Unbind(GridPlotModel model)
        {
            return model;
        }

        static Object Unbind(OxyPlotModel model)
        {
            var view = model.Model.PlotView as PlotView;

            if (view != null)
            {
                view.Model = null;
            }

            return model;
        }
    }
}
