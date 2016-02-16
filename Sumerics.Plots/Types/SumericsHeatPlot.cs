namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System;
    using YAMP;

    sealed class SumericsHeatPlot : SumericsOxyPlot
    {
		#region Fields

		readonly HeatmapPlotValue _plot;
        HeatMapSeries _series;

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

		public override Boolean IsGridEnabled
		{
			get { return false; }
		}

		public override Boolean IsSeriesEnabled
		{
			get { return true; }
		}

		#endregion

        #region Helpers

        void UpdateSeries(HeatMapSeries series, IPointSeries points)
        {
            series.Title = points.Label;
            //series.HeatmapColors = GenerateColors(_plot.ColorPalette, 50);
        }

		void SetSeries(PlotModel model)
		{
            for (var i = 0; i < _plot.Count; i++)
            {
                var points = _plot[i];
                _series = new HeatMapSeries();
                //(Int32)_plot.MaxX, (Int32)_plot.MaxY, points
                //UpdateSeries(ser, points);
                //model.Series.Add(ser);
            }
		}

		void SetProperties(PlotModel model)
        {
            //_series.IsInterpolated = _plot.IsInterpolated;

			model.Axes.Add(new OxyPlot.Axes.LinearAxis());
			model.Axes.Add(new OxyPlot.Axes.LinearAxis());

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
            //_series.IsInterpolated = _plot.IsInterpolated;

			model.Axes[0].Title = _plot.XLabel;
			model.Axes[1].Title = _plot.YLabel;

			model.Axes[0].Minimum = _plot.MinX;
			model.Axes[0].Maximum = _plot.MaxX;

			model.Axes[1].Minimum = _plot.MinY;
			model.Axes[1].Maximum = _plot.MaxY;
		}

		#endregion
    }
}
