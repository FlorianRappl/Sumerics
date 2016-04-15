namespace Sumerics.Plots
{
    using System;
    using System.Collections.Generic;
    using YAMP;

	abstract class SumericsPlot : IPlotController
	{
		#region Fields

        static readonly Dictionary<String, Action<SumericsPlot>> handlers = new Dictionary<String, Action<SumericsPlot>>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "data", c => c.UpdateData() },
            { "series", c => c.UpdateSeries() },
            { "properties", c => c.UpdateProperties() }
        };

		readonly PlotValue _plot;

		#endregion

		#region ctor

		public SumericsPlot(PlotValue plot)
		{
			_plot = plot;
			_plot.OnPlotChanged += PlotValueChanged;
		}

		#endregion

        #region Properties

        public virtual Boolean IsSeriesEnabled
		{
			get { return true; }
		}

        public virtual Boolean IsGridEnabled
		{
			get { return true; }
		}

		public PlotValue Plot
		{
		   get { return _plot; }
		}

		#endregion

        #region Methods

        protected abstract void UpdateProperties();

        protected abstract void UpdateSeries();

        protected abstract void UpdateData();

        #endregion

        #region Plot Changed

        void Update()
        {
            UpdateProperties();
            UpdateData();
            UpdateSeries();
        }

        void PlotValueChanged(Object sender, PlotEventArgs e)
        {
            var handler = default(Action<SumericsPlot>);

            if (handlers.TryGetValue(e.PropertyName, out handler))
            {
                handler.Invoke(this);
            }
            else
            {
                Update();
            }
        }

		#endregion
    }
}
