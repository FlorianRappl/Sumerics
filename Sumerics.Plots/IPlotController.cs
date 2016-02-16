namespace Sumerics.Plots
{
    using System;
    using YAMP;

    public interface IPlotController
    {
        IPlotControl Control { get; set; }

        Boolean IsGridEnabled { get; }

        Boolean IsPreview { get; }

        Boolean IsSeriesEnabled { get; }

        PlotValue Plot { get; }
    }
}
