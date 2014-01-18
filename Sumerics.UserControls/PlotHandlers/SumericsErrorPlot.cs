using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using YAMP;

namespace Sumerics.Controls
{
	class SumericsErrorPlot : SumericsOxyPlot
	{
		#region Members

		ErrorPlotValue _plot;

		#endregion

		#region ctor

		public SumericsErrorPlot(ErrorPlotValue plot) : base(plot)
		{
			_plot = plot;
			SetSeries(Model);
			SetProperties(Model);
		}

		#endregion

		#region Methods

		void SetSeries(PlotModel model)
		{
			for (var i = 0; i < _plot.Count; i++)
			{
				var points = _plot[i];
				var series = new ErrorSeries();

				for (var j = 0; j < points.Count; j++)
				{
					series.Points.Add(new ErrorItem
					{
						X = points[j].X,
						Y = points[j].Y,
						XError = points[j].Xerr,
						YError = points[j].Yerr
					});
				}

				UpdateLineSeries(series, points);
				model.Series.Add(series);
			}
		}

		void SetProperties(PlotModel model)
		{
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

		void UpdateProperties(PlotModel model)
		{
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

		#endregion
	}
}
