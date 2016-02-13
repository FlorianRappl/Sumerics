namespace Sumerics.Controls
{
    using YAMP;

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
