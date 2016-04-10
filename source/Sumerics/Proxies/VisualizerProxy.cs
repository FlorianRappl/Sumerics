namespace Sumerics.Proxies
{
    using Sumerics.Controls;
    using Sumerics.Dialogs;
    using Sumerics.Plots;
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;
    using YAMP;

    sealed class VisualizerProxy : IVisualizer
    {
        readonly IConsole _console;
        PlotControl _plotter;

        public VisualizerProxy(IConsole console)
        {
            _console = console;
        }

        public PlotControl Plotter
        {
            get { return _plotter ?? (_plotter = GetPlotter()); }
        }

        public void Dock()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var window = DialogExtensions.Get<PlotWindow>();

                if (window != null)
                {
                    //SetPlot(window.Controller);
                    window.Close();
                }
            });
        }

        public void Dock(Object context)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var window = DialogExtensions.Get<PlotWindow>();

                //if (window != null && Object.ReferenceEquals(window.Controller.Plot, context))
                //{
                //    SetPlot(window.Controller);
                //    window.Close();
                //}
            });
        }

        public void Undock()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                //Plotter.Undock();
            });
        }

        public void Undock(Object context)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                //var plotter = Plotter;

                //if (plotter.Data == null || !Object.ReferenceEquals(plotter.Data.Plot, context))
                //{
                //    var model = new PlotViewModel((PlotValue)context, this, _console);
                //    model.UndockPlot();
                //}
                //else
                //{
                //    plotter.Undock();
                //}
            });
        }

        static void SetPlot(IPlotController controller)
        {
            var window = App.Current.MainWindow as MainWindow;

            if (window != null)
            {
                var vm = window.DataContext as MainViewModel;

                if (vm != null)
                {
                    //vm.LastPlot = controller;
                }
            }
        }

        static PlotControl GetPlotter()
        {
            var window = App.Current.MainWindow as MainWindow;

            if (window != null)
            {
                return window.MyLastPlot;
            }

            return null;
        }
    }
}
