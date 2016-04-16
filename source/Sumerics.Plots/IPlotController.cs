namespace Sumerics.Plots
{
    using System;
    using YAMP;

    public interface IPlotController
    {
        PlotValue Plot { get; }

        Object Model { get; }

        void CenterPlot();

        void ToggleGrid();
    }
}
