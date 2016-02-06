namespace Sumerics.Controls
{
    using OxyPlot;
    using OxyPlot.Axes;
    using YAMP;

	class SumericsComplexPlot : SumericsOxyPlot
	{
		#region Fields

		ComplexPlotValue _plot;

		#endregion

		#region ctor

		public SumericsComplexPlot(ComplexPlotValue plot) : base(plot)
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
				return false;
			}
		}

		#endregion

		#region Methods

		void SetSeries(PlotModel model)
		{
			var series = new ComplexSeries(_plot.Fz);
			model.Series.Add(series);
		}

		void SetProperties(PlotModel model)
		{
			model.Axes.Add(new LinearAxis());
			model.Axes.Add(new LinearAxis());
			model.Axes[0].Position = AxisPosition.Bottom;
			model.Axes[0].Minimum = _plot.MinX;
			model.Axes[0].Maximum = _plot.MaxX;
			model.Axes[0].Title = _plot.XLabel;
			model.Axes[1].Position = AxisPosition.Left;
			model.Axes[1].Minimum = _plot.MinY;
			model.Axes[1].Maximum = _plot.MaxY;
			model.Axes[1].Title = _plot.YLabel;
		}

		void UpdateProperties(PlotModel model)
		{
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
