namespace Sumerics
{
    using Sumerics.Controls;
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;
    using YAMP;

    sealed class VisualizerProxy : IVisualizer
    {
        readonly MainViewModel _viewModel;
        readonly PlotControl _plotter;

        public VisualizerProxy(MainViewModel viewModel, PlotControl plotter)
        {
            _viewModel = viewModel;
            _plotter = plotter;
        }

        public void Dock()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var win = default(PlotWindow);

                foreach (var window in App.Current.Windows)
                {
                    if (window is PlotWindow)
                    {
                        win = (PlotWindow)window;
                    }
                }

                if (win != null)
                {
                    _viewModel.LastPlot = win.PlotModel;
                    win.Close();
                }
            });
        }

        public void Dock(Object context)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var window in App.Current.Windows)
                {
                    if (window is PlotWindow)
                    {
                        var win = (PlotWindow)window;

                        if (Object.ReferenceEquals(win.PlotModel.Plot, context))
                        {
                            _viewModel.LastPlot = win.PlotModel;
                            win.Close();
                        }
                    }
                }
            });
        }

        public void Undock()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                _plotter.Undock();
            });
        }

        public void Undock(Object context)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (_plotter.Data == null || !Object.ReferenceEquals(_plotter.Data.Plot, context))
                {
                    var app = _viewModel.Container.Get<IApplication>();
                    var model = new PlotViewModel(context as PlotValue, app);
                    model.UndockPlot();
                }
                else
                {
                    _plotter.Undock();
                }
            });
        }
    }
}
