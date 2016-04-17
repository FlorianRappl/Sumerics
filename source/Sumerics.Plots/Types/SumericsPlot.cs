namespace Sumerics.Plots
{
    using System;
    using System.Collections.Generic;
    using YAMP;

	abstract class SumericsPlot : IPlotController
	{
		#region Fields

        static readonly Dictionary<String, Action<SumericsPlot>> handlers = new Dictionary<String, Action<SumericsPlot>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Data", c => c.UpdateSeries() },
            { "Layout", c => c.UpdateProperties() },
            { "Properties", c => c.UpdateSeries() }
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

        public abstract Object Model
        {
            get;
        }

		public PlotValue Plot
		{
		   get { return _plot; }
		}

		#endregion

        #region Methods

        public void CenterPlot()
        {
            OnCenterPlot();
        }

        public void ToggleGrid()
        {
            OnToggleGrid();
        }

        protected virtual void OnCenterPlot()
        {
        }

        protected virtual void OnToggleGrid()
        {
        }

        protected abstract void UpdateProperties();

        protected abstract void UpdateSeries();

        protected abstract void Refresh();

        #endregion

        #region Plot Changed

        void Update()
        {
            UpdateProperties();
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

            Refresh();
        }

		#endregion
    }
}
