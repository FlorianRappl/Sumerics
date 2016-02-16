namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using YAMP;

	sealed class SumericsBarPlot : SumericsOxyPlot
	{
		#region Fields

		readonly BarPlotValue _plot;

		#endregion

		#region ctor

		public SumericsBarPlot(BarPlotValue plot) : 
            base(plot)
		{
			_plot = plot;
			SetSeries(Model);
			SetProperties(Model);
		}

		#endregion

        #region Helpers

        void SetSeries(PlotModel model)
		{
			for (var i = 0; i < _plot.Count; i++)
			{
				var points = _plot[i];
				var series = new ColumnSeries();

                foreach (var point in points)
                {
                    series.Items.Add(new ColumnItem
                    {
                        Color = points.Color.OxyColorFromString(),
                        Value = point
                    });
                }

				UpdateSeries(series, points);
				model.Series.Add(series);
			}
		}

		void UpdateSeries(ColumnSeries series, BarPlotValue.BarPoints points)
		{
            series.StrokeThickness = points.Lines ? points.LineWidth : 0.0;
			series.ColumnWidth = points.BarWidth;
			series.StrokeColor = OxyColors.Black;
			series.Title = points.Label;
            var color = points.Color.OxyColorFromString();
            series.FillColor = color;

            foreach (var item in series.Items)
            {
                item.Color = color;
            }
		}

		void SetProperties(PlotModel model)
		{
			var major = _plot.Gridlines ? LineStyle.Solid : LineStyle.None;
			var minor = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;
			model.Axes.Add(new CategoryAxis());
			model.Axes.Add(new LinearAxis());
			model.Axes[0].MajorGridlineStyle = major;
			model.Axes[0].MinorGridlineStyle = minor;
			model.Axes[0].Position = AxisPosition.Bottom;
			model.Axes[0].Title = _plot.XLabel;
			model.Axes[1].MajorGridlineStyle = major;
			model.Axes[1].MinorGridlineStyle = minor;
			model.Axes[1].Position = AxisPosition.Left;
			model.Axes[1].Minimum = _plot.MinY;
			model.Axes[1].Maximum = _plot.MaxY;
			model.Axes[1].Title = _plot.YLabel;
		}

		void UpdateProperties(PlotModel model)
		{
			var major = _plot.Gridlines ? LineStyle.Solid : LineStyle.None;
			var minor = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;

			model.Axes[0].MajorGridlineStyle = major;
			model.Axes[0].MinorGridlineStyle = minor;
			model.Axes[0].Title = _plot.XLabel;

			model.Axes[1].MajorGridlineStyle = major;
			model.Axes[1].MinorGridlineStyle = minor;
			model.Axes[1].Title = _plot.YLabel;

			model.Axes[1].Minimum = _plot.MinY;
			model.Axes[1].Maximum = _plot.MaxY;
		}

		#endregion
	}
}
