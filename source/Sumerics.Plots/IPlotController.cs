namespace Sumerics.Plots
{
    using System;
    using YAMP;

    public interface IPlotController
    {
        Boolean IsGridEnabled { get; }

        Boolean IsSeriesEnabled { get; }

        PlotValue Plot { get; }
    }
}
