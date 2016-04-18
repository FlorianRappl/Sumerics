namespace Sumerics.Plots
{
    using Sumerics.Plots.Models;
    using System;
    using YAMP;

	abstract class Sumerics3DPlot : SumericsPlot
    {
        #region Fields

        protected readonly WpfPlotModel _model;
        readonly XYZPlotValue _plot;

        #endregion

        #region ctor

        public Sumerics3DPlot(XYZPlotValue plot)
            : base(plot)
        {
            _model = new WpfPlotModel
            {
                CanEditSeries = false,
                CanToggleGrid = true,
                IsAxisShown = false,
                Transformation = new Transform3dModel(),
            };
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

        protected override void OnCenterPlot()
        {
            _model.Transformation.Reset();
        }

        protected override void OnToggleGrid()
        {
            _model.IsAxisShown = !_model.IsAxisShown;
        }

        protected override void Refresh()
        {
            UpdateSeries();
        }

        #endregion
    }
}
