﻿namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
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
                    var point = new DataPoint(points[j].Magnitude, points[j].Angle);
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

        #endregion
    }
}
