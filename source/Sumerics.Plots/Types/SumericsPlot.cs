namespace Sumerics.Plots
{
    using System;
    using System.Collections.Generic;
    using YAMP;

	abstract class SumericsPlot : IPlotController
	{
		#region Fields

        static readonly Dictionary<String, Action<IPlotControl>> handlers = new Dictionary<String, Action<IPlotControl>>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "data", c => c.RefreshData() },
            { "series", c => c.RefreshSeries() },
            { "properties", c => c.RefreshSeries() }
        };

		readonly PlotValue _plot;
        IPlotControl _control;

		#endregion

		#region ctor

		public SumericsPlot(PlotValue plot)
		{
			_plot = plot;
			_plot.OnPlotChanged += PlotValueChanged;
		}

		#endregion

        #region Properties

        public IPlotControl Control
        {
            get { return _control; }
            set { _control = value; }
        }

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

        #region Plot Changed

        protected abstract void UpdateProperties();

        void PlotValueChanged(Object sender, PlotEventArgs e)
        {
            var source = e.PropertyName;
            var handler = default(Action<IPlotControl>);
            var control = _control;

            if (control != null)
            {
                if (handlers.TryGetValue(source, out handler))
                {
                    handler(control);
                }
                else
                {
                    control.RefreshProperties();
                }
            }
        }

		#endregion
    }
}
