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

        Boolean _isLogx;
        Boolean _isLogy;
        Boolean _isLogz;

        #endregion

        #region ctor

        public Sumerics3DPlot(XYZPlotValue plot)
            : base(plot)
        {
            _model = new WpfPlotModel
            {
                CanEditSeries = false,
                CanToggleGrid = false
            };
            _plot = plot;
        }

        #endregion

        #region Properties

        public override Object Model
        {
            get { return _model; }
        }

        public Boolean IsLogX
        {
            get { return _isLogx; }
            protected set { _isLogx = value; }
        }

        public Boolean IsLogY
        {
            get { return _isLogy; }
            protected set { _isLogy = value; }
        }

        public Boolean IsLogZ
        {
            get { return _isLogz; }
            protected set { _isLogz = value; }
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
