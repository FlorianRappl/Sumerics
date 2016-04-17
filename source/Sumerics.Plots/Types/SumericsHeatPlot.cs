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
        readonly LinearColorAxis _colorAxes;

		#endregion

		#region ctor

        public SumericsHeatPlot(HeatmapPlotValue plot)
            : base(plot)
        {
            var model = _model.Model;
            _plot = plot;
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            _colorAxes = new LinearColorAxis { Position = AxisPosition.Right };
            model.Axes.Add(_colorAxes);
			UpdateSeries();
			UpdateProperties();
		}

		#endregion

        #region Methods

        protected override void UpdateCustomProperties()
        {
            var model = _model.Model;
            var colors = _plot.ColorPalette.GenerateColors(50);

            model.Axes[0].Title = _plot.XLabel;
            model.Axes[1].Title = _plot.YLabel;

            model.Axes[0].Minimum = _plot.MinX;
            model.Axes[0].Maximum = _plot.MaxX;

            model.Axes[1].Minimum = _plot.MinY;
            model.Axes[1].Maximum = _plot.MaxY;

            _colorAxes.Palette = new OxyPalette(colors);
            _colorAxes.Minimum = _plot.Minimum;
            _colorAxes.Maximum = _plot.Maximum;
        }

        protected override void UpdateSeries()
        {
            var model = _model.Model;
            model.Series.Clear();

            for (var i = 0; i < _plot.Count; i++)
            {
                var points = _plot[i];
                var series = new HeatMapSeries();
                UpdateSeries(series, points);
                model.Series.Add(series);
            }
        }

        #endregion

        #region Helpers

        void UpdateSeries(HeatMapSeries series, Points<HeatmapPlotValue.HeatPoint> points)
        {
            series.Title = points.Label;
            series.Interpolate = _plot.IsInterpolated;
            series.X0 = _plot.MinX;
            series.X1 = _plot.MaxX;
            series.Y0 = _plot.MinY;
            series.Y1 = _plot.MaxY;
            var cols = (Int32)_plot.MaxX;
            var rows = (Int32)_plot.MaxY;
            var data = new Double[cols, rows];

            foreach (var point in points)
            {
                data[point.Column - 1, point.Row - 1] = point.Magnitude;
            }

            series.Data = data;
        }

		#endregion
    }
}
