namespace Sumerics.Controls.Plots
{
    using System;
    using System.IO;

    /// <summary>
    /// Interaction logic for Oxy2dPlotControl.xaml
    /// </summary>
    public partial class Oxy2dPlotControl : OxyPlotControl
    {
        public Oxy2dPlotControl()
        {
            InitializeComponent();
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
        /*
        // HEATMAP:

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                //var series = (HeatmapSeries)Model.Series[i];
                //UpdateSeries(series, data);
            }

            Refresh();
        }

        // ERRROR:

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                var series = (LineSeries)Model.Series[i];
                UpdateLineSeries(series, data);
            }

            Refresh();
        }

        // CONTOUR:

        public override void PreviewMode()
        {
            base.AsPreview();

            foreach (ContourSeries cs in Model.Series)
            {
                cs.FontSize = 0;
                cs.LabelBackground = OxyColors.Transparent;
            }
        }

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                var series = (ContourSeries)Model.Series[i];
                UpdateSeries(series, data);
            }

            Refresh();
        }

        // COMPLEX:

        // POLAR:

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                var series = (LineSeries)Model.Series[i];
                UpdateLineSeries(series, data);
            }

            Refresh();
        }

        // CLASSIC (2D):

        public override void RefreshSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                var data = _plot.GetSeries(i);
                var series = (LineSeries)Model.Series[i];
                UpdateLineSeries(series, data);
            }

            Refresh();
        }
         */
    }
}
