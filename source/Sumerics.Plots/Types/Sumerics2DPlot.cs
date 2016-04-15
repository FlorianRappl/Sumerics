namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using YAMP;

    sealed class Sumerics2DPlot : SumericsOxyPlot, I2dPlotController
	{
		#region Fields

		readonly Plot2DValue _plot;

		#endregion

		#region ctor

		public Sumerics2DPlot(Plot2DValue plot) : 
            base(plot)
		{
			_plot = plot;
			SetSeries();
			SetProperties();
		}

		#endregion

        #region Methods

        protected override void UpdateProperties()
        {
            var model = Model;
            var major = _plot.Gridlines ? LineStyle.Solid : LineStyle.None;
            var minor = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;

            if (_plot.IsLogX && model.Axes[0] is LinearAxis)
            {
                model.Axes[0] = new LogarithmicAxis { Position = AxisPosition.Bottom };
            }
            else if (!_plot.IsLogX && model.Axes[0] is LogarithmicAxis)
            {
                model.Axes[0] = new LinearAxis { Position = AxisPosition.Bottom };
            }

            if (_plot.IsLogY && model.Axes[1] is LinearAxis)
            {
                model.Axes[1] = new LogarithmicAxis { Position = AxisPosition.Left };
            }
            else if (!_plot.IsLogY && model.Axes[1] is LogarithmicAxis)
            {
                model.Axes[1] = new LinearAxis { Position = AxisPosition.Left };
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

        void SetSeries()
		{
            var model = Model;

			for (var i = 0; i < _plot.Count; i++)
			{
				var points = _plot[i];
				var series = new LineSeries();

				for (var j = 0; j < points.Count; j++)
				{
                    var point = new DataPoint(points[j].X, points[j].Y);
					series.Points.Add(point);
				}

				UpdateLineSeries(series, points);
				model.Series.Add(series);
			}
		}

		void SetProperties()
		{
            var model = Model;
			var major = _plot.Gridlines ? LineStyle.Solid : LineStyle.None;
			var minor = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;
			model.Axes.Add(_plot.IsLogX ? (Axis)new LogarithmicAxis() : new LinearAxis());
			model.Axes.Add(_plot.IsLogY ? (Axis)new LogarithmicAxis() : new LinearAxis());
			model.Axes[0].MajorGridlineStyle = major;
			model.Axes[0].MinorGridlineStyle = minor;
			model.Axes[0].Position = AxisPosition.Bottom;
			model.Axes[0].Minimum = _plot.MinX;
			model.Axes[0].Maximum = _plot.MaxX;
			model.Axes[0].Title = _plot.XLabel;
			model.Axes[1].MajorGridlineStyle = major;
			model.Axes[1].MinorGridlineStyle = minor;
			model.Axes[1].Position = AxisPosition.Left;
			model.Axes[1].Minimum = _plot.MinY;
			model.Axes[1].Maximum = _plot.MaxY;
			model.Axes[1].Title = _plot.YLabel;
		}

		#endregion
	}
}
