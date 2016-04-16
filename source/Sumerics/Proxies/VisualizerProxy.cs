namespace Sumerics.Proxies
{
    using Sumerics.Dialogs;
    using Sumerics.ViewModels;
    using Sumerics.Views;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YAMP;

    sealed class VisualizerProxy : IVisualizer
    {
        readonly List<PlotViewModel> _contexts;
        readonly IConsole _console;

        public VisualizerProxy(IConsole console)
        {
            _contexts = new List<PlotViewModel>();
            _console = console;
        }

        public void Show(Object obj)
        {
            var plot = obj as PlotValue;
            var vm = GetViewModel();

            if (plot != null && vm != null)
            {
                var context = new PlotViewModel(plot, this, _console);
                _contexts.Add(context);
                vm.LastPlot = context;
            }
        }

        public Object HideCurrent()
        {
            var context = _contexts.LastOrDefault();
            var vm = GetViewModel();

            if (context != null && vm != null)
            {
                _contexts.Remove(context);
                vm.LastPlot = _contexts.LastOrDefault();
            }

            return context;
        }

        public void Dock(Object obj)
        {
            var context = ConvertToViewModel(obj);

            if (context != null)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    DialogExtensions.GetAll<PlotWindow>().Where(m => Object.ReferenceEquals(m.DataContext, context)).ForEach(window =>
                    {
                        Show(context);
                        window.Close();
                    });
                });
            }
        }

        public void DockLast()
        {
            var window = DialogExtensions.GetAll<PlotWindow>().LastOrDefault();

            if (window != null)
            {
                Dock(window.DataContext);
            }
        }

        public void Undock()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var context = HideCurrent();

                if (context != null)
                {
                    var window = new PlotWindow { DataContext = context };
                    window.Show();
                }
            });
        }

        public void UndockAny(Object obj)
        {
            var context = ConvertToViewModel(obj);

            if (context != null)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (Object.ReferenceEquals(context, _contexts.LastOrDefault()))
                    {
                        Undock();
                    }
                    else if (_contexts.Remove(context))
                    {
                        var window = new PlotWindow { DataContext = context };
                        window.Show();
                    }
                });
            }
        }

        PlotViewModel ConvertToViewModel(Object obj)
        {
            if (obj is PlotValue)
            {
                return _contexts.FirstOrDefault(m => Object.ReferenceEquals(m.Plot, obj)) ??
                    DialogExtensions.GetAll<PlotWindow>().
                        Select(m => m.DataContext as PlotViewModel).
                        Where(m => m != null).
                        FirstOrDefault(m => Object.ReferenceEquals(m.Plot, obj));
            }

            return obj as PlotViewModel;
        }

        static MainViewModel GetViewModel()
        {
            var window = App.Current.MainWindow as MainWindow;

            if (window != null)
            {
                return window.DataContext as MainViewModel;
            }

            return default(MainViewModel);
        }
    }
}
