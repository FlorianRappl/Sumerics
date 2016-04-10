namespace Sumerics.Plots
{
    using System;
    using System.Collections.Generic;
    using YAMP;

    sealed class SumericsSubPlot : SumericsPlot
    {
        #region Fields

        readonly List<SumericsPlot> _subplots;
        readonly SubPlotValue _plot;

        #endregion

        #region ctor

        public SumericsSubPlot(SubPlotValue plot) 
            : base(plot)
        {
            _subplots = new List<SumericsPlot>();
            _plot = plot;
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
    }
}
