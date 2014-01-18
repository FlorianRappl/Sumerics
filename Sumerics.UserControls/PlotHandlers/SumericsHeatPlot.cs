using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;

namespace Sumerics.Controls 
{
    class SumericsHeatPlot : SumericsOxyPlot
    {
		#region Members

		HeatmapPlotValue _plot;
        HeatmapSeries ser;

		#endregion

		#region ctor

        public SumericsHeatPlot(HeatmapPlotValue plot)
            : base(plot)
		{
			_plot = plot;
			SetSeries(Model);
			SetProperties(Model);
		}

		#endregion

		#region Properties

		public override bool IsGridEnabled
		{
			get
			{
				return false;
			}
		}

		public override bool IsSeriesEnabled
		{
			get
			{
				return true;
			}
		}

		#endregion

        #region Methods

        void UpdateSeries(HeatmapSeries series, IPointSeries points)
        {
            series.Title = points.Label;
            series.HeatmapColors = GenerateColors(_plot.ColorPalette, 50);
        }

		void SetSeries(PlotModel model)
		{
            for (var i = 0; i < _plot.Count; i++)
            {
                var points = _plot[i];
                ser = new HeatmapSeries((int)_plot.MaxX, (int)_plot.MaxY, points);
                UpdateSeries(ser, points);
                model.Series.Add(ser);
            }
		}

		void SetProperties(PlotModel model)
        {
            ser.IsInterpolated = _plot.IsInterpolated;

			model.Axes.Add(new LinearAxis());
			model.Axes.Add(new LinearAxis());

            model.Axes[0].Position = AxisPosition.Bottom;
            model.Axes[1].Position = AxisPosition.Left;

			model.Axes[0].Minimum = _plot.MinX - 1.0;
			model.Axes[0].Maximum = _plot.MaxX;
			model.Axes[0].Title = _plot.XLabel;

            model.Axes[1].Minimum = _plot.MinY - 1.0;
            model.Axes[1].Maximum = _plot.MaxY;
            model.Axes[1].Title = _plot.YLabel;

            model.Axes[1].StartPosition = 1;
            model.Axes[1].EndPosition = 0;
		}

		void UpdateProperties(PlotModel model)
        {
            ser.IsInterpolated = _plot.IsInterpolated;

			model.Axes[0].Title = _plot.XLabel;
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
                var series = (HeatmapSeries)Model.Series[i];
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
