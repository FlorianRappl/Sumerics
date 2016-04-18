namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System;
    using System.Collections.Generic;
    using YAMP;

	sealed class SumericsContourPlot : SumericsOxyPlot
	{
		#region Fields

		readonly ContourPlotValue _plot;

		#endregion

		#region ctor

		public SumericsContourPlot(ContourPlotValue plot) : 
            base(plot)
        {
            var model = _model.Model;
            _plot = plot;
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
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
                var n = 0;
                var points = _plot[i];
                var series = new ContourSeries();
                var y = new List<Double>();
                var x = new List<Double>();

                for (var j = 0; j < points.Count; j++)
                {
                    if (y.Count == 0 || points[j].X > y[y.Count - 1])
                    {
                        y.Add(points[j].X);
                    }

                    if (x.Count == 0 || points[j].Y > x[x.Count - 1])
                    {
                        x.Add(points[j].Y);
                    }
                }

                var z = new Double[y.Count, x.Count];

                for (var k = 0; k < y.Count; k++)
                {
                    for (var l = 0; l < x.Count; l++)
                    {
                        z[k, l] = points[n++].Magnitude;
                    }
                }

                series.RowCoordinates = y.ToArray();
                series.ColumnCoordinates = x.ToArray();
                series.Data = z;
                series.ContourLevels = _plot.Levels;
                series.LabelFormatString = "0.000";
                UpdateSeries(series, points);
                model.Series.Add(series);
            }
        }

        protected override void UpdateCustomProperties()
        {
            var model = _model.Model;
            var major = _plot.Gridlines ? LineStyle.Solid : LineStyle.None;
            var minor = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;

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

		void UpdateSeries(ContourSeries series, IPointSeries points)
		{
			series.StrokeThickness = points.LineWidth;
			series.Title = points.Label;
			series.ContourColors = _plot.ColorPalette.GenerateOxyColors(_plot.Levels.Length);

			if (_plot.ShowLevel)
			{
				series.FontSize = Double.NaN;
				series.LabelBackground = OxyColor.FromArgb(220, 255, 255, 255);
			}
			else
			{
				series.FontSize = 0;
				series.LabelBackground = OxyColors.Transparent;
			}

			series.CalculateContours();
		}

		#endregion
    }
}
