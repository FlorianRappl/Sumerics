namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using Sumerics.Plots.Models;
    using System;
    using YAMP;

	abstract class SumericsOxyPlot : SumericsPlot
    {
        #region Fields

        protected readonly OxyPlotModel _model;
        readonly XYPlotValue _plot;

        #endregion

        #region ctor

        public SumericsOxyPlot(XYPlotValue plot) : 
            base(plot)
		{
            _plot = plot;
            var model = new PlotModel();
            _model = new OxyPlotModel { Model = model };
            model.PlotMargins = new OxyThickness(0);
            model.Padding = new OxyThickness(0, 10, 10, 0);
		}

        #endregion

        #region Properties

        public override Object Model
		{
			get { return _model; }
		}

        #endregion

        #region Methods

        protected override void Refresh()
        {
            var model = _model.Model;

            if (model.PlotView != null)
            {
                model.InvalidatePlot(false);
            }
        }

        protected sealed override void UpdateProperties()
        {
            var model = _model.Model;
            model.Title = _plot.Title;
            model.IsLegendVisible = _plot.ShowLegend;
            model.LegendBackground = _plot.LegendBackground.OxyColorFromString();
            model.LegendBorderThickness = _plot.LegendLineWidth;
            model.LegendBorder = _plot.LegendLineColor.OxyColorFromString();
            model.LegendPosition = (OxyPlot.LegendPosition)_plot.LegendPosition;
            UpdateCustomProperties();
        }

        protected abstract void UpdateCustomProperties();

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

        #endregion

        #region Helpers

        protected static Axis Axis(Boolean log, AxisPosition position)
        {
            if (log)
            {
                return new LogarithmicAxis { Position = position };
            }

            return new LinearAxis { Position = position };
        }

        #endregion
    }
}
