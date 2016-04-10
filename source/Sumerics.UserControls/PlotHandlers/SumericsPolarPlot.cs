namespace Sumerics.Controls
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using YAMP;

	class SumericsPolarPlot : SumericsOxyPlot
	{
		PolarPlotValue _plot;

		public SumericsPolarPlot(PolarPlotValue plot) : base(plot)
		{
            _plot = plot;
            SetSeries(Model);
            SetProperties(Model);
		}

		void SetSeries(PlotModel model)
		{
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

		void SetProperties(PlotModel model)
		{
			var major = _plot.Gridlines ? LineStyle.Solid : LineStyle.None;
			var minor = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;
			model.PlotAreaBorderThickness = new OxyThickness(0);
			model.PlotType = PlotType.Polar;

			model.Axes.Add(new AngleAxis
			{
                StartAngle = _plot.MinX,
                EndAngle = _plot.MaxX,
                MinorStep = _plot.MaxX / 16.0,
                MajorStep = _plot.MaxX / 8.0,
				MajorGridlineStyle = major,
				MinorGridlineStyle = minor,
				FormatAsFractions = true,
				FractionUnit = _plot.FractionUnit,
				FractionUnitSymbol = _plot.FractionSymbol
			});

			model.Axes.Add(new MagnitudeAxis()
			{
				MajorGridlineStyle = major,
				MinorGridlineStyle = minor,
				Minimum = _plot.MinY,
				Maximum = _plot.MaxY
			});
		}

		void UpdateProperties(PlotModel model)
		{
			var major = _plot.Gridlines ? LineStyle.Solid : LineStyle.None;
			var minor = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;

			var angle = model.Axes[0] as AngleAxis;
			angle.MajorGridlineStyle = major;
			angle.MinorGridlineStyle = minor;
			angle.FractionUnit = _plot.FractionUnit;
			angle.FractionUnitSymbol = _plot.FractionSymbol;
			angle.Minimum = _plot.MinX;
			angle.Maximum = _plot.MaxX;

			var magnitude = model.Axes[1] as MagnitudeAxis;
			magnitude.MajorGridlineStyle = major;
			magnitude.MinorGridlineStyle = minor;
			magnitude.Minimum = _plot.MinY;
			magnitude.Maximum = _plot.MaxY;
		}

		public override void RefreshData()
		{
			Model.Series.Clear();
			SetSeries(Model);
			Refresh();
		}

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

		public override void RefreshProperties()
		{
			SetGeneralProperties(Model);
			UpdateProperties(Model);
			Refresh();
		}
	}
}
