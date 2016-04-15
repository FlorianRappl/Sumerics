namespace Sumerics.Plots
{
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
            var model = Model;
            _plot = plot;
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
			UpdateSeries();
			UpdateProperties();
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

        #region Methods

        protected override void UpdateProperties()
        {
            var model = Model;
            //_series.IsInterpolated = _plot.IsInterpolated;

            model.Axes[0].Title = _plot.XLabel;
            model.Axes[1].Title = _plot.YLabel;

            model.Axes[0].Minimum = _plot.MinX;
            model.Axes[0].Maximum = _plot.MaxX;

            model.Axes[1].Minimum = _plot.MinY;
            model.Axes[1].Maximum = _plot.MaxY;
        }

        protected override void UpdateSeries()
        {
            var model = Model;
            model.Series.Clear();

            for (var i = 0; i < _plot.Count; i++)
            {
                var points = _plot[i];
                _series = new HeatMapSeries();
                //(Int32)_plot.MaxX, (Int32)_plot.MaxY, points
                //UpdateSeries(ser, points);
                //model.Series.Add(ser);
            }
        }

        protected override void UpdateData()
        {
        }

        #endregion

        #region Helpers

        void UpdateSeries(HeatMapSeries series, IPointSeries points)
        {
            series.Title = points.Label;
            //series.HeatmapColors = GenerateColors(_plot.ColorPalette, 50);
        }

		#endregion
    }
}
