namespace Sumerics.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using YAMP;

	public abstract class SumericsPlot
	{
		#region Members

		PlotValue _plot;
		Boolean _preview;

		#endregion

		#region ctor

		public SumericsPlot(PlotValue plot)
		{
			_plot = plot;
			_plot.OnPlotChanged += PlotValueChanged;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the current status of this control.
		/// </summary>
        public Boolean IsPreview
		{
			get { return _preview; }
			protected set { _preview = value; }
		}

		/// <summary>
		/// Gets whether the series button should be enabled.
		/// </summary>
        public virtual Boolean IsSeriesEnabled
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether the grid button should be enabled.
		/// </summary>
        public virtual Boolean IsGridEnabled
		{
			get { return true; }
		}

		/// <summary>
		/// Gets the currently assigned plot to this control.
		/// </summary>
		public PlotValue Plot
		{
		   get { return _plot; }
		}

		/// <summary>
		/// Gets the current plot control.
		/// </summary>
		public abstract FrameworkElement Content
		{
			get;
		}

		#endregion

		#region Methods

        public abstract void RenderToCanvas(Canvas canvas);

		void PlotValueChanged(Object sender, PlotEventArgs e)
		{
			var source = e.PropertyName;

			Content.Dispatcher.Invoke((Action)(() =>
			{
				switch (source)
				{
					case "Data":
						RefreshData();
						break;
					case "Series":
					case "Properties":
						RefreshSeries();
						break;
					default:
                        if (!IsPreview)
                        {
                            RefreshProperties();
                        }
						break;
				}
			}));
		}

        //public abstract FrameworkElement CopyUI();

		/// <summary>
		/// Resets the plot to the center.
		/// </summary>
        public abstract void CenterPlot();

        /// <summary>
        /// Toggles the grid lines.
        /// </summary>
        public abstract void ToggleGrid();

		/// <summary>
		/// Saves the plot in the file with the specified name.
		/// </summary>
		/// <param name="fileName">Where to save the file.</param>
		/// <param name="width">The width in px.</param>
		/// <param name="height">The height in px.</param>
        public abstract void ExportPlot(String fileName, Int32 width, Int32 height);

		/// <summary>
		/// Creates the Control to use in preview mode.
		/// </summary>
		public abstract void AsPreview();

		/// <summary>
		/// Refreshs the data of the plot i.e. x, y, etc. values.
		/// </summary>
		public abstract void RefreshData();

		/// <summary>
		/// Refreshs the properties of the series of the plot, i.e. color, label etc.
		/// </summary>
		public abstract void RefreshSeries();

		/// <summary>
		/// Refreshs the properties of the plot, i.e. gridlines, axes labels, title, ...
		/// </summary>
		public abstract void RefreshProperties();

		#endregion
	}
}
