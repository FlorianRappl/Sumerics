namespace Sumerics.ViewModels
{
    using Sumerics.Models;
    using Sumerics.Plots;
    using Sumerics.Resources;
    using Sumerics.Views;
    using System;
    using System.IO;
    using System.Windows;
    using YAMP;

    public sealed class PlotViewModel : BaseViewModel
    {
        #region Fields

        static readonly PlotFactory ControllerFactory = new PlotFactory();

        readonly IVisualizer _visualizer;
        readonly PlotValue _plot;
        readonly IConsole _console;
        readonly IPlotController _controller;

        #endregion

        #region ctor

        public PlotViewModel(PlotValue plot, IVisualizer visualizer, IConsole console)
        {
            _visualizer = visualizer;
            _console = console;
            _plot = plot;
            _controller = ControllerFactory.Create(plot);
        }

        #endregion

        #region Properties

        public IPlotController Controller
        {
            get { return _controller; }
        }

        #endregion

        #region Methods

        public void OpenConsole()
        {
            var context = new ConsoleEnterViewModel(_console);
            ShowDialog<ConsoleEnterWindow>(context);
        }

        public void OpenPlotSettings()
        {
            if (_plot is XYPlotValue)
            {
                var context = new PlotSettingsViewModel((XYPlotValue)_plot);
                ShowDialog<PlotSettingsWindow>(context);
            }
            else if (_plot is SubPlotValue)
            {
                var context = new SubPlotSettingsViewModel((SubPlotValue)_plot);
                ShowDialog<SubPlotSettingsWindow>(context);
            }
        }

        public void OpenPlotSeries()
        {
            if (_plot is ContourPlotValue)
            {
                var context = new ContourViewModel((ContourPlotValue)_plot);
                ShowDialog<ContourSeriesWindow>(context);
            }
            else if (_plot is HeatmapPlotValue)
            {
                var context = new HeatmapViewModel((HeatmapPlotValue)_plot);
                ShowDialog<HeatSeriesWindow>(context);
            }
            else if (_plot is XYPlotValue)
            {
                var context = new SeriesViewModel((XYPlotValue)_plot);
                ShowDialog<PlotSeriesWindow>(context);
            }
        }

        public void UndockPlot()
        {
            _visualizer.Undock(_plot);
        }

        public void DockPlot()
        {
            _visualizer.Dock(_plot);
        }

        public void SavePlot()
        {
            var context = new SaveImageViewModel();
            context.ImageWidth = 640;
            context.ImageHeight = 480;
            var window = new SaveImageWindow { DataContext = context };
            window.Title = Messages.SavePlotAs;

            if (!String.IsNullOrEmpty(_plot.Title))
            {
                context.SelectedFile = new FileModel(_plot.Title);
            }

            window.ShowDialog();

            if (context.Accepted)
            {
                var path = context.SelectedFile.FullName;
                //frame.ExportPlot(path, context.ImageWidth, context.ImageHeight);
                var filename = Path.GetFileName(path);
                var message = String.Format(Messages.PlotSavedMessage, filename);
                var output = new OutputViewModel 
                { 
                    Title = Messages.FileCreated, 
                    Message = message 
                };
                output.ShowWindow();
            }
        }

        #endregion

        #region Helpers

        static void ShowDialog<TWindow>(Object context)
            where TWindow : Window, new()
        {
            var window = new TWindow { DataContext = context };
            window.ShowDialog();
        }

        #endregion
    }
}
