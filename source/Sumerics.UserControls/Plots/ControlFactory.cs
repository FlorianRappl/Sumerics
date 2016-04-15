namespace Sumerics.Controls.Plots
{
    using Sumerics.Plots;

    public sealed class ControlFactory : TypeFactory<IPlotController, BasePlotControl>
    {
        public ControlFactory()
            : base(false)
        {
            Register<I3dPlotController>(controller => new Wpf3dPlotControl());
            Register<I2dPlotController>(controller => new Oxy2dPlotControl());
            Register<IGridPlotController>(controller => new GridPlotControl());
            Register<IBarPlotController>(controller => new OxyBarPlotControl());
        }

        protected override BasePlotControl CreateDefault()
        {
            return null;
        }
    }
}
