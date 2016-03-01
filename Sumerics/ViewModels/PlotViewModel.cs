namespace Sumerics.ViewModels
{
    using Sumerics.Models;
    using Sumerics.Resources;
    using Sumerics.Views;
    using System;
    using System.IO;
    using YAMP;

    sealed class PlotViewModel : BaseViewModel
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
            var context = new ConsoleEnterViewModel(_console);
            var window = new ConsoleEnterWindow { DataContext = context };
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

        public void SavePlot()
        {
            var context = new SaveImageViewModel();
            context.ImageWidth = 640;
            context.ImageHeight = 480;
            var dialog = new SaveImageWindow { DataContext = context };
            dialog.Title = Messages.SavePlotAs;

            if (!String.IsNullOrEmpty(_plot.Title))
            {
                context.SelectedFile = new FileModel(_plot.Title);
            }

            dialog.ShowDialog();

            if (context.Accepted)
            {
                var path = context.SelectedFile.FullName;
                //frame.ExportPlot(path, context.ImageWidth, context.ImageHeight);
                var filename = Path.GetFileName(path);
                var info = String.Format(Messages.PlotSavedMessage, filename);
                OutputDialog.Show(Messages.FileCreated, info);
            }
        }

        #endregion
    }
}
