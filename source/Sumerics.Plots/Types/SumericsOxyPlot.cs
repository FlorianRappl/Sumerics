namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Series;
    using YAMP;

	abstract class SumericsOxyPlot : SumericsPlot
    {
        #region Fields

        readonly PlotModel _model;
        readonly XYPlotValue _plot;

        #endregion

        #region ctor

        public SumericsOxyPlot(XYPlotValue plot) : 
            base(plot)
		{
            _plot = plot;
            _model = new PlotModel();
            SetGeneralProperties(_model);
		}

        #endregion

        #region Properties

        public PlotModel Model
		{
			get { return _model; }
		}

        #endregion

        #region Some very general modifiers

        protected void SetGeneralProperties(PlotModel model)
		{
			model.Title = _plot.Title;
			model.PlotMargins = new OxyThickness(0);
			model.Padding = new OxyThickness(0, 10, 10, 0);
			model.IsLegendVisible = _plot.ShowLegend;
			model.LegendBackground = _plot.LegendBackground.OxyColorFromString();
			model.LegendBorderThickness = _plot.LegendLineWidth;
			model.LegendBorder = _plot.LegendLineColor.OxyColorFromString();
			model.LegendPosition = (OxyPlot.LegendPosition)_plot.LegendPosition;
		}

        protected void UpdateLineSeries(XYAxisSeries series, IPointSeries points)
        {
            series.Title = points.Label;
            //series.StrokeThickness = points.Lines ? points.LineWidth : 0.0;
            //series.Color = points.Color.OxyColorFromString();
            //series.MarkerType = (MarkerType)((int)points.Symbol);
            //series.MarkerFill = series.Color;
            //series.MarkerSize = 3.0;
            //series.MarkerStroke = series.Color;
            //series.MarkerStrokeThickness = 1.0;
		}

		protected void Refresh()
		{
            if (Model.PlotView != null)
            {
                Model.InvalidatePlot(false);
            }
		}

        #endregion
    }
}
