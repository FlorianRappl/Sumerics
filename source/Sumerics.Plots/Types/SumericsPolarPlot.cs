namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System;
    using YAMP;

	sealed class SumericsPolarPlot : SumericsOxyPlot
    {
        #region Fields

        readonly PolarPlotValue _plot;

        #endregion

        #region ctor

        public SumericsPolarPlot(PolarPlotValue plot) : 
            base(plot)
        {
            var model = _model.Model;
            model.PlotAreaBorderThickness = new OxyThickness(0);
            model.PlotType = PlotType.Polar;
            _plot = plot;
            model.Axes.Add(new AngleAxis { FormatAsFractions = true });
            model.Axes.Add(new MagnitudeAxis());
            UpdateSeries();
            UpdateProperties();
		}

        #endregion

        #region Methods

        protected override void UpdateCustomProperties()
        {
            const Double ConvertToDegree = 180.0 / Math.PI;

            var model = _model.Model;
            var gridline = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;

            var angle = model.Axes[0] as AngleAxis;
            angle.MajorGridlineStyle = LineStyle.Solid;
            angle.MinorGridlineStyle = gridline;
            angle.FractionUnit = _plot.FractionUnit;
            angle.FractionUnitSymbol = _plot.FractionSymbol;
            angle.StartAngle = _plot.MinX * ConvertToDegree;
            angle.EndAngle = _plot.MaxX * ConvertToDegree;
            angle.MinorStep = angle.EndAngle / 16.0;
            angle.MajorStep = angle.EndAngle / 8.0;

            var magnitude = model.Axes[1] as MagnitudeAxis;
            magnitude.MajorGridlineStyle = LineStyle.Solid;
            magnitude.MinorGridlineStyle = gridline;
            magnitude.Minimum = _plot.MinY;
            magnitude.Maximum = _plot.MaxY;
        }

        protected override void OnToggleGrid()
        {
            _plot.MinorGridlines = !_plot.MinorGridlines;
            _plot.UpdateLayout();
        }

        protected override void UpdateSeries()
        {
            var model = _model.Model;
            model.Series.Clear();

            for (var i = 0; i < _plot.Count; i++)
            {
                var points = _plot[i];
                var series = new LineSeries();

                for (var j = 0; j < points.Count; j++)
                {
                    var point = new DataPoint(points[j].Magnitude, points[j].Angle);
                    series.Points.Add(point);
                }

                UpdateLineSeries(series, points);
                model.Series.Add(series);
            }
        }

        #endregion
    }
}
