namespace Sumerics.Api
{
    using YAMP;

    [Description("This function is useful if you want to preserve a plot by duplicating it. Duplicating a plot results in a new window with the given plot.")]
    [Kind("UI")]
    sealed class UndockFunction : ArgumentFunction
    {
        readonly IApplication _application;

        public UndockFunction(IApplication application)
        {
            _application = application;
        }

        [Description("Undocks the current plot from the main window.")]
        [Example("undock()", "Duplicates the currently selected plot.")]
        public void Function()
        {
            var visualizer = _application.Get<IVisualizer>();
            visualizer.Undock();
        }

        [Description("Undocks the given plot from the main window.")]
        [Example("undock(cplot(sin))", "Creates a new complex plot and undocks it immediately.")]
        public void Function(PlotValue plot)
        {
            var visualizer = _application.Get<IVisualizer>();
            visualizer.Undock(plot);
        }
    }
}
