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

        readonly IVisualizer _visualizer;
        readonly PlotValue _plot;
        readonly IConsole _console;

        #endregion

        #region ctor

        public PlotViewModel(PlotValue plot, IVisualizer visualizer, IConsole console)
        {
            _visualizer = visualizer;
            _console = console;
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
            var window = new ConsoleEnterWindow(_console);
            window.ShowDialog();
        }

        public void OpenPlotSettings()
        {
            if (_plot is XYPlotValue)
            {
                var vm = new PlotSettingsViewModel((XYPlotValue)_plot);
                var window = new PlotSettingsWindow(vm);
                window.ShowDialog();
            }
            else if (_plot is SubPlotValue)
            {
                var vm = new SubPlotSettingsViewModel((SubPlotValue)_plot);
                var window = new SubPlotSettingsWindow(vm);
                window.ShowDialog();
            }
        }

        public void OpenPlotSeries()
        {
            if (_plot is ContourPlotValue)
            {
                var vm = new ContourViewModel((ContourPlotValue)_plot);
                var window = new ContourSeriesWindow(vm);
                window.ShowDialog();
            }
            else if (_plot is HeatmapPlotValue)
            {
                var vm = new HeatmapViewModel((HeatmapPlotValue)_plot);
                var window = new HeatSeriesWindow(vm);
                window.ShowDialog();
            }
            else if (_plot is XYPlotValue)
            {
                var vm = new SeriesViewModel((XYPlotValue)_plot);
                var window = new PlotSeriesWindow(vm);
                window.ShowDialog();
            }
        }

        public void UndockPlot()
        {
            _visualizer.Undock(Plot);
        }

        public void DockPlot()
        {
            _visualizer.Dock(Plot);
        }

        public void SavePlot(SumericsPlot frame)
        {
            var vm = new SaveImageViewModel(Environment.CurrentDirectory);
            var dialog = new SaveImageWindow(vm);
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
