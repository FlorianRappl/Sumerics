namespace Sumerics.ViewModels
{
    using Sumerics.Controls.Plots;
    using Sumerics.Models;
    using Sumerics.Plots;
    using Sumerics.Resources;
    using Sumerics.Views;
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using YAMP;

    public sealed class PlotViewModel : BaseViewModel
    {
        #region Fields

        static readonly PlotFactory ControllerFactory = PlotFactory.Instance;

        readonly IPlotController _controller;
        readonly ICommand _center;
        readonly ICommand _console;
        readonly ICommand _undock;
        readonly ICommand _dock;
        readonly ICommand _grid;
        readonly ICommand _save;
        readonly ICommand _series;
        readonly ICommand _print;
        readonly ICommand _settings;

        #endregion

        #region ctor

        public PlotViewModel(PlotValue plot, IVisualizer visualizer, IConsole console)
        {
            _controller = ControllerFactory.Create(plot);
            _console = new RelayCommand(_ => ShowConsole(console));
            _settings = new RelayCommand(_ => ShowSettings(plot));
            _series = new RelayCommand(_ => ShowSeries(plot));
            _grid = new RelayCommand(_ => _controller.ToggleGrid());
            _center = new RelayCommand(_ =>_controller.CenterPlot());
            _dock = new RelayCommand(_ => visualizer.Dock(this));
            _undock = new RelayCommand(_ => visualizer.Undock());
            _print = new RelayCommand(obj => Print(plot, obj as IPlotSerializer));
            _save = new RelayCommand(obj => Save(plot, obj as IPlotSerializer));
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

        public ICommand CenterPlot
        {
            get { return _center; }
        }

        public ICommand OpenConsole
        {
            get { return _console; }
        }

        public ICommand UndockPlot
        {
            get { return _undock; }
        }

        public ICommand DockPlot
        {
            get { return _dock; }
        }

        public ICommand ToggleGrid
        {
            get { return _grid; }
        }

        public ICommand PrintPlot
        {
            get { return _print; }
        }

        public ICommand SavePlot
        {
            get { return _save; }
        }

        public ICommand EditSeries
        {
            get { return _series; }
        }

        public ICommand OpenSettings
        {
            get { return _settings; }
        }

        #endregion

        #region Helpers

        static void ShowConsole(IConsole console)
        {
            var context = new ConsoleEnterViewModel(console);
            ShowDialog<ConsoleEnterWindow>(context);
        }

        static void ShowSettings(PlotValue plot)
        {
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

        static void ShowSeries(PlotValue plot)
        {
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

        static void Print(PlotValue plot, IPlotSerializer plotter)
        {
            var printDialog = new PrintDialog();
            var result = printDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                plotter.Print(printDialog, plot.Title ?? "Sumerics Plot");
            }
        }

        static void Save(PlotValue plot, IPlotSerializer plotter)
        {
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
                var filename = Path.GetFileName(path);
                var message = String.Format(Messages.PlotSavedMessage, filename);
                var output = new OutputViewModel
                {
                    Title = Messages.FileCreated,
                    Message = message
                };

                using (var fs = File.Create(path))
                {
                    plotter.Save(fs, context.ImageWidth, context.ImageHeight);
                }

                output.ShowWindow();
            }
        }

        static void ShowDialog<TWindow>(Object context)
            where TWindow : Window, new()
        {
            var window = new TWindow { DataContext = context };
            window.ShowDialog();
        }

        #endregion
    }
}
