namespace Sumerics.Plots
{
    using OxyPlot.Axes;
    using System;
    using YAMP;

	sealed class SumericsComplexPlot : SumericsOxyPlot
	{
		#region Fields

		readonly ComplexPlotValue _plot;

		#endregion

		#region ctor

		public SumericsComplexPlot(ComplexPlotValue plot) : 
            base(plot)
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
			get { return false; }
		}

		#endregion

        #region Methods

        protected override void UpdateSeries()
        {
            var model = Model;
            var series = new ComplexSeries(_plot.Fz);
            model.Series.Clear();
            model.Series.Add(series);
        }

        protected override void UpdateData()
        {
        }

        protected override void UpdateProperties()
        {
            var model = Model;

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
