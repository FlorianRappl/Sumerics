namespace Sumerics.Controls.Plots
{
    using System;
    using System.IO;

    /// <summary>
    /// Interaction logic for OxyPlotControl.xaml
    /// </summary>
    public partial class OxyPlotControl : BasePlotControl
    {
        public OxyPlotControl()
        {
            InitializeComponent();
        }

        /*
        public override void RenderToCanvas(Canvas canvas)
        {
            //var rc = new OxyPlot.Wpf.CanvasRenderContext(canvas);
            //model.Render(rc);
        }

        public override void ExportPlot(string fileName, int width, int height)
        {
            control.SaveBitmap(fileName, width, height, OxyColors.White);
        }

        public override void AsPreview()
        {
            IsPreview = true;

            _model.PlotAreaBorderThickness = new OxyThickness(0);
            _model.PlotMargins = new OxyThickness(0);
            _model.Padding = new OxyThickness(0);
            _model.IsLegendVisible = false;
            _model.Axes[0].IsAxisVisible = false;
            _model.Axes[1].IsAxisVisible = false;

            _model.Axes[0].IsZoomEnabled = false;
            _model.Axes[1].IsZoomEnabled = false;
            control.IsManipulationEnabled = false;

            control.PlotAreaBorderThickness = new Thickness(0);
            control.PlotMargins = new Thickness(0);
            control.Padding = new Thickness(0);
            control.Margin = new Thickness(0);
        }
         */

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
         
        // BAR PLOT:

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
    }
}
