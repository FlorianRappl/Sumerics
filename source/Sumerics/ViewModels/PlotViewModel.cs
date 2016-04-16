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
        readonly IConsole _console;
        readonly IPlotController _controller;

        #endregion

        #region ctor

        public PlotViewModel(PlotValue plot, IVisualizer visualizer, IConsole console)
        {
            _visualizer = visualizer;
            _console = console;
            _controller = ControllerFactory.Create(plot);
        }

        #endregion

        #region Properties

        public Object Model
        {
            get { return _controller.Model; }
        }

        public PlotValue Plot
        {
            get { return _controller.Plot; }
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
            var plot = Plot;

            if (plot is XYPlotValue)
            {
                var context = new PlotSettingsViewModel((XYPlotValue)plot);
                ShowDialog<PlotSettingsWindow>(context);
            }
            else if (plot is SubPlotValue)
            {
                var context = new SubPlotSettingsViewModel((SubPlotValue)plot);
                ShowDialog<SubPlotSettingsWindow>(context);
            }
        }

        public void OpenPlotSeries()
        {
            var plot = Plot;

            if (plot is ContourPlotValue)
            {
                var context = new ContourViewModel((ContourPlotValue)plot);
                ShowDialog<ContourSeriesWindow>(context);
            }
            else if (plot is HeatmapPlotValue)
            {
                var context = new HeatmapViewModel((HeatmapPlotValue)plot);
                ShowDialog<HeatSeriesWindow>(context);
            }
            else if (plot is XYPlotValue)
            {
                var context = new SeriesViewModel((XYPlotValue)plot);
                ShowDialog<PlotSeriesWindow>(context);
            }
        }

        public void UndockPlot()
        {
            _visualizer.Undock();
        }

        public void DockPlot()
        {
            _visualizer.Dock(this);
        }

        public void SavePlot()
        {
            var plot = Plot;
            var context = new SaveImageViewModel();
            context.ImageWidth = 640;
            context.ImageHeight = 480;
            var window = new SaveImageWindow { DataContext = context };
            window.Title = Messages.SavePlotAs;

            if (!String.IsNullOrEmpty(plot.Title))
            {
                context.SelectedFile = new FileModel(plot.Title);
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
