namespace Sumerics.Plots
{
    using Sumerics.Plots.Models;
    using System;
    using System.Collections.Generic;
    using YAMP;

    sealed class SumericsSubPlot : SumericsPlot
    {
        #region Fields

        readonly GridPlotModel _model;
        readonly List<SumericsPlot> _subplots;
        readonly SubPlotValue _plot;

        #endregion

        #region ctor

        public SumericsSubPlot(SubPlotValue plot) 
            : base(plot)
        {
            _model = new GridPlotModel();
            _subplots = new List<SumericsPlot>();
            _plot = plot;
        }

        #endregion

        #region Properties

        public override Object Model
        {
            get { return _model; }
        }

        #endregion

        #region Methods

        protected override void UpdateSeries()
        {
            //TODO Update Model
        }

        protected override void UpdateData()
        {
            //TODO Update Model
        }

        protected override void UpdateProperties()
        {
            //TODO Update Model
        }

        #endregion

        #region Methods

        protected override void Refresh()
        {
            //TODO
        }

        #endregion
    }
}
