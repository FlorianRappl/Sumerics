using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using YAMP;

namespace Sumerics.Controls
{
	class SumericsContourPlot : SumericsOxyPlot
	{
		#region Members

		ContourPlotValue _plot;

		#endregion

		#region ctor

		public SumericsContourPlot(ContourPlotValue plot) : base(plot)
		{
			_plot = plot;
			SetSeries(Model);
			SetProperties(Model);
		}

		#endregion

		#region Methods

		public override void AsPreview()
		{
			base.AsPreview();

			foreach(ContourSeries cs in Model.Series)
			{
				cs.FontSize = 0;
				cs.LabelBackground = OxyColors.Transparent;
			}
		}

		void SetSeries(PlotModel model)
		{
			for (var i = 0; i < _plot.Count; i++)
			{
				var n = 0;
				var points = _plot[i];
				var series = new ContourSeries();
				var y = new List<double>();
				var x = new List<double>();

				for (var j = 0; j < points.Count; j++)
				{
					if (y.Count == 0 || points[j].X > y[y.Count - 1])
						y.Add(points[j].X);

					if (x.Count == 0 || points[j].Y > x[x.Count - 1])
						x.Add(points[j].Y);
				}

				var z = new double[y.Count, x.Count];

				for (var k = 0; k < y.Count; k++)
					for (var l = 0; l < x.Count; l++)
						z[k, l] = points[n++].Magnitude;

				series.RowCoordinates = y.ToArray();
				series.ColumnCoordinates = x.ToArray();
				series.Data = z;
				series.ContourLevels = _plot.Levels;
				series.LabelFormatString = "0.000";
				UpdateSeries(series, points);
				model.Series.Add(series);
			}
		}

		void UpdateSeries(ContourSeries series, IPointSeries points)
		{
			series.StrokeThickness = points.LineWidth;
			series.Title = points.Label;
			series.ContourColors = GenerateColors(_plot.ColorPalette, _plot.Levels.Length);

			if (_plot.ShowLevel && !IsPreview)
			{
				series.FontSize = double.NaN;
				series.LabelBackground = OxyColor.FromArgb(220, 255, 255, 255);
			}
			else
			{
				series.FontSize = 0;
				series.LabelBackground = OxyColors.Transparent;
			}

			series.CalculateContours();
		}

		void SetProperties(PlotModel model)
		{
			var major = _plot.Gridlines ? LineStyle.Solid : LineStyle.None;
			var minor = _plot.MinorGridlines ? LineStyle.Solid : LineStyle.None;
			model.Axes.Add(new LinearAxis());
			model.Axes.Add(new LinearAxis());
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

			model.Axes[0].Minimum = _plot.MinX;
			model.Axes[0].Maximum = _plot.MaxX;
			model.Axes[1].Minimum = _plot.MinY;
			model.Axes[1].Maximum = _plot.MaxY;
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
				var series = (ContourSeries)Model.Series[i];
				UpdateSeries(series, data);
			}

			Refresh();
		}

		public override void RefreshProperties()
		{
			SetGeneralProperties(Model);
			UpdateProperties(Model);
			Refresh();
		}

		#endregion
	}
}
