namespace Sumerics.Plots
{
    using YAMP;

	public sealed class PlotFactory : TypeFactory<PlotValue, IPlotController>
	{
        public PlotFactory()
            : base(true)
        {
            Register<Plot2DValue>(plot => new Sumerics2DPlot(plot));
            Register<PolarPlotValue>(plot => new SumericsPolarPlot(plot));
            Register<ContourPlotValue>(plot => new SumericsContourPlot(plot));
            Register<BarPlotValue>(plot => new SumericsBarPlot(plot));
            Register<ErrorPlotValue>(plot => new SumericsErrorPlot(plot));
            Register<ComplexPlotValue>(plot => new SumericsComplexPlot(plot));
            Register<HeatmapPlotValue>(plot => new SumericsHeatPlot(plot));
            Register<SubPlotValue>(plot => new SumericsSubPlot(plot));
            Register<Plot3DValue>(plot => new SumericsLinePlot3D(plot));
            Register<SurfacePlotValue>(plot => new SumericsSurfacePlot(plot));
        }

        public override IPlotController CreateDefault()
        {
            return null;
        }
    }
}
