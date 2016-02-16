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
            { "data", plot => plot.RefreshData() },
            { "series", plot => plot.RefreshSeries() },
            { "properties", plot => plot.RefreshSeries() }
        };

		readonly PlotValue _plot;
		Boolean _preview;
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

        public Boolean IsPreview
		{
			get { return _preview; }
			protected set { _preview = value; }
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

		#region Methods

        public virtual void ActivatePreview()
        {
            if (_control != null)
            {
                _control.AsPreview();
            }
        }

        #endregion

        #region Plot Changed

        void PlotValueChanged(Object sender, PlotEventArgs e)
        {
            var source = e.PropertyName;
            var handler = default(Action<SumericsPlot>);

            if (handlers.TryGetValue(source, out handler))
            {
                handler(this);
            }
            else if (!IsPreview)
            {
                RefreshProperties();
            }
        }

		void RefreshData()
        {
            if (_control != null)
            {
                _control.RefreshData();
            }
        }

        void RefreshSeries()
        {
            if (_control != null)
            {
                _control.RefreshSeries();
            }
        }

        void RefreshProperties()
        {
            if (_control != null)
            {
                _control.RefreshProperties();
            }
        }

		#endregion
    }
}
