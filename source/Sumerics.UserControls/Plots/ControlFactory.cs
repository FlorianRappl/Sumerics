namespace Sumerics.Controls.Plots
{
    using Sumerics.Plots.Models;
    using System;

    public sealed class ControlFactory : TypeFactory<Object, BasePlotControl>
    {
        public ControlFactory()
            : base(true)
        {
            Register<OxyPlotModel>(model => new Oxy2dPlotControl { DataContext = model });
            Register<GridPlotModel>(model => new GridPlotControl { DataContext = model });
            Register<WpfPlotModel>(model => new Wpf3dPlotControl { DataContext = model });
        }

        protected override BasePlotControl CreateDefault()
        {
            return null;
        }
    }
}
