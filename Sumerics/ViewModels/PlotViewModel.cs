namespace Sumerics.ViewModels
{
    using Sumerics.Controls;
    using Sumerics.Views;
    using System;
    using System.IO;
    using YAMP;

    sealed class PlotViewModel : BaseViewModel, IPlotViewModel
    {
        #region Fields

        readonly IApplication _app;
        readonly PlotValue _plot;

        #endregion

        #region ctor

        public PlotViewModel(PlotValue plot, IApplication app)
        {
            _app = app;
            _plot = plot;
        }

        #endregion

        #region Properties

        public PlotValue Plot
        {
            get { return _plot; }
        }

        #endregion

        #region Methods

        public void OpenConsole()
        {
            var window = new ConsoleEnterWindow(_app.Console);
            window.ShowDialog();
        }

        public void OpenPlotSettings()
        {
            if (_plot is XYPlotValue)
            {
                var window = new PlotSettingsWindow((XYPlotValue)_plot);
                window.ShowDialog();
            }
            else if (_plot is SubPlotValue)
            {
                var window = new SubPlotSettingsWindow((SubPlotValue)_plot);
                window.ShowDialog();
            }
        }

        public void OpenPlotSeries()
        {
            if (_plot is ContourPlotValue)
            {
                var window = new ContourSeriesWindow((ContourPlotValue)_plot);
                window.ShowDialog();
            }
            else if (_plot is HeatmapPlotValue)
            {
                var window = new HeatSeriesWindow((HeatmapPlotValue)_plot);
                window.ShowDialog();
            }
            else if (_plot is XYPlotValue)
            {
                var window = new PlotSeriesWindow((XYPlotValue)_plot);
                window.ShowDialog();
            }
        }

        public void UndockPlot()
        {
            _app.Visualizer.Undock(Plot);
        }

        public void DockPlot()
        {
            _app.Visualizer.Dock(Plot);
        }

        public void SavePlot(SumericsPlot frame)
        {
            var dialog = new SaveImageWindow();
            dialog.ImageWidth = 640;
            dialog.ImageHeight = 480;
            dialog.Title = "Save plot as ...";

            if (!String.IsNullOrEmpty(_plot.Title))
            {
                dialog.SelectedFile = _plot.Title;
            }

            dialog.ShowDialog();

            if (dialog.Accepted)
            {
                frame.ExportPlot(dialog.SelectedFile, dialog.ImageWidth, dialog.ImageHeight);
                var filename = Path.GetFileName(dialog.SelectedFile);
                var info = String.Format("The plot has been successfully saved in the file {0}.", filename);
                OutputDialog.Show("File created", info);
            }
        }

        #endregion
    }
}
