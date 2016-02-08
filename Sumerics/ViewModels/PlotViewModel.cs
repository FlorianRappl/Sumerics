namespace Sumerics
{
    using Sumerics.Controls;
    using System;
    using System.IO;
    using YAMP;

    sealed class PlotViewModel : BaseViewModel, IPlotViewModel
    {
        #region Fields

        PlotValue _plot;

        #endregion

        #region ctor

        public PlotViewModel(PlotValue plot, IContainer container)
            : base(container)
        {
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
            new ConsoleEnterWindow().ShowDialog();
        }

        public void OpenPlotSettings()
        {
            if (_plot is XYPlotValue)
            {
                var window = new PlotSettingsWindow((XYPlotValue)_plot, Container);
                window.ShowDialog();
            }
            else if (_plot is SubPlotValue)
            {
                var window = new SubPlotSettingsWindow((SubPlotValue)_plot, Container);
                window.ShowDialog();
            }
        }

        public void OpenPlotSeries()
        {
            if (_plot is ContourPlotValue)
            {
                var window = new ContourSeriesWindow((ContourPlotValue)_plot, Container);
                window.ShowDialog();
            }
            else if (_plot is HeatmapPlotValue)
            {
                var window = new HeatSeriesWindow((HeatmapPlotValue)_plot, Container);
                window.ShowDialog();
            }
            else if (_plot is XYPlotValue)
            {
                var window = new PlotSeriesWindow((XYPlotValue)_plot, Container);
                window.ShowDialog();
            }
        }

        public void UndockPlot()
        {
            PlotWindow.Show(this);
        }

        public void DockPlot()
        {
            foreach (var window in App.Current.Windows)
            {
                if (window is PlotWindow)
                {
                    var win = (PlotWindow)window;

                    if (win.PlotModel == this)
                    {
                        win.Close();
                    }
                }
            }
                    
            App.Window.DockImage(this);
        }

        public void SavePlot(SumericsPlot frame)
        {
            var dialog = new SaveImageWindow(Container);
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
                OutputDialog.Show("File created", String.Format("The plot has been successfully saved in the file {0}.",
                    Path.GetFileName(dialog.SelectedFile)));
            }
        }

        #endregion
    }
}
