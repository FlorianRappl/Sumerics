using System;
using System.Windows.Input;
using YAMP;

namespace Sumerics.Controls
{
    public interface IPlotViewModel
    {
        void UndockPlot();

        void DockPlot();

        void OpenConsole();

        void OpenPlotSeries();

        void OpenPlotSettings();

        void SavePlot(SumericsPlot frame);

        PlotValue Plot { get; }
    }
}
