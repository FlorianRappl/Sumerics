namespace Sumerics.Controls.Plots
{
    public class OxyPlotControl : BasePlotControl
    {
        /*
        public override void RenderToCanvas(Canvas canvas)
        {
            //var rc = new OxyPlot.Wpf.CanvasRenderContext(canvas);
            //model.Render(rc);
        }

        public override void CenterPlot()
        {
            control.ResetAllAxes();
        }

        public override void ExportPlot(string fileName, int width, int height)
        {
            control.SaveBitmap(fileName, width, height, OxyColors.White);
        }

        public override void ToggleGrid()
        {
            if (_plot.Gridlines)
            {
                _plot.Gridlines = false;
                _plot.MinorGridlines = false;
            }
            else
            {
                _plot.Gridlines = true;
                _plot.MinorGridlines = true;
            }

            _plot.UpdateLayout();
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
    }
}
