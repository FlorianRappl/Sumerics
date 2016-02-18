namespace Sumerics.Controls.Plots
{
    using Sumerics.Plots;

    public sealed class ControlFactory : TypeFactory<IPlotController, IPlotControl>
    {
        public ControlFactory()
            : base(true)
        {
            Register<I3dPlotController>(controller => new Wpf3dPlotControl(controller));
            Register<I2dPlotController>(controller => new Oxy2dPlotControl(controller));
            Register<IGridPlotController>(controller => new GridPlotControl(controller));
            Register<IBarPlotController>(controller => new OxyBarPlotControl(controller));
        }

        protected override IPlotControl CreateDefault()
        {
            return null;
        }
    }
}
