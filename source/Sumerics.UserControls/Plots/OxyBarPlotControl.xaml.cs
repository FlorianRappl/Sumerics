namespace Sumerics.Controls.Plots
{
    using System;
    using System.IO;

    /// <summary>
    /// Interaction logic for OxyBarPlotControl.xaml
    /// </summary>
    public partial class OxyBarPlotControl : OxyPlotControl
    {
        public OxyBarPlotControl()
        {
            InitializeComponent();
        }

        /*
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
