namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using YAMP;

	sealed class SumericsErrorPlot : SumericsOxyPlot
	{
		#region Fields

		readonly ErrorPlotValue _plot;

		#endregion

		#region ctor

		public SumericsErrorPlot(ErrorPlotValue plot) : 
            base(plot)
		{
            var model = _model.Model;
            _plot = plot;
            model.Axes.Add(Axis(_plot.IsLogX, AxisPosition.Bottom));
            model.Axes.Add(Axis(_plot.IsLogY, AxisPosition.Left));
			UpdateSeries();
			UpdateProperties();
		}

		#endregion

        #region Methods

        protected override void UpdateSeries()
        {
            var model = _model.Model;
            model.Series.Clear();

            for (var i = 0; i < _plot.Count; i++)
            {
                var points = _plot[i];
                var series = new ScatterErrorSeries();

                for (var j = 0; j < points.Count; j++)
                {
                    var point = new ScatterErrorPoint(points[j].X, points[j].Y, points[j].Xerr, points[j].Yerr);
                    series.Points.Add(point);
                }

                UpdateScatterSeries(series, points);
                model.Series.Add(series);
            }
        }

        protected override void UpdateCustomProperties()
        {
            var model = _model.Model;
            var major = _plot.Gridlines ? LineStyle.Solid : LineStyle.None;
            var minor = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;

            if (_plot.IsLogX && model.Axes[0] is LinearAxis)
            {
                model.Axes[0] = new LogarithmicAxis();
                model.Axes[0].Position = AxisPosition.Bottom;
            }
            else if (!_plot.IsLogX && model.Axes[0] is LogarithmicAxis)
            {
                model.Axes[0] = new LinearAxis();
                model.Axes[0].Position = AxisPosition.Bottom;
            }

            if (_plot.IsLogY && model.Axes[1] is LinearAxis)
            {
                model.Axes[1] = new LogarithmicAxis();
                model.Axes[1].Position = AxisPosition.Bottom;
            }
            else if (!_plot.IsLogY && model.Axes[1] is LogarithmicAxis)
            {
                model.Axes[1] = new LinearAxis();
                model.Axes[1].Position = AxisPosition.Bottom;
            }

            model.Axes[0].MajorGridlineStyle = major;
            model.Axes[0].MinorGridlineStyle = minor;
            model.Axes[0].Title = _plot.XLabel;

            model.Axes[1].MajorGridlineStyle = major;
            model.Axes[1].MinorGridlineStyle = minor;
            model.Axes[1].Title = _plot.YLabel;

            model.Axes[0].Minimum = _plot.MinX;
            model.Axes[0].Maximum = _plot.MaxX;
            model.Axes[1].Minimum = _plot.MinY;
            model.Axes[1].Maximum = _plot.MaxY;
        }

        #endregion

        #region Helpers

        void UpdateScatterSeries(ScatterErrorSeries series, IPointSeries points)
        {
            UpdateXySeries(series, points);
            series.ErrorBarColor = points.Color.OxyColorFromString();
            series.MarkerType = (MarkerType)((int)points.Symbol);
            series.MarkerFill = series.ErrorBarColor;
            series.MarkerSize = 3.0;
            series.MarkerStroke = series.ErrorBarColor;
            series.MarkerStrokeThickness = 1.0;
        }

        #endregion
    }
}
