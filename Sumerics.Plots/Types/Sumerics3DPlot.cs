namespace Sumerics.Plots
{
    using System;
    using YAMP;

	abstract class Sumerics3DPlot : SumericsPlot, I3dPlotController
    {
        #region Fields

        readonly XYZPlotValue _plot;

        Boolean _isLogx;
        Boolean _isLogy;
        Boolean _isLogz;

        #endregion

        #region ctor

        public Sumerics3DPlot(XYZPlotValue plot)
            : base(plot)
        {
            _plot = plot;
        }

        #endregion

        #region Properties

        public override Boolean IsGridEnabled
        {
            get { return true; }
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
    }
}
