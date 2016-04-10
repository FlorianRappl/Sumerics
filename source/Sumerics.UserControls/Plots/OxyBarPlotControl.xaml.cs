namespace Sumerics.Controls.Plots
{
    using Sumerics.Plots;
    using System;
    using System.IO;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for OxyBarPlotControl.xaml
    /// </summary>
    public partial class OxyBarPlotControl : OxyPlotControl, IPlotControl
    {
        public OxyBarPlotControl(IBarPlotController controller)
        {
            InitializeComponent();
            controller.Control = this;
        }

        /*
        public void RefreshData()
        {
            Model.Series.Clear();
            SetSeries(Model);
            Refresh();
        }

        public void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = (BarPlotValue.BarPoints)_plot.GetSeries(i);
                var series = (ColumnSeries)Model.Series[i];
                UpdateSeries(series, data);
            }

            Refresh();
        }

        public void RefreshProperties()
        {
            SetGeneralProperties(Model);
            UpdateProperties(Model);
            Refresh();
        }
         */

        public void ToggleGrid()
        {
            throw new NotImplementedException();
        }

        public void PreviewMode()
        {
            throw new NotImplementedException();
        }

        public void CenterPlot()
        {
            throw new NotImplementedException();
        }

        public void ExportAsPng(Stream stream, Int32 width, Int32 height)
        {
            throw new NotImplementedException();
        }

        public void RefreshData()
        {
            throw new NotImplementedException();
        }

        public void RefreshProperties()
        {
            throw new NotImplementedException();
        }

        public void RefreshSeries()
        {
            throw new NotImplementedException();
        }
    }
}
