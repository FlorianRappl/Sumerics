namespace Sumerics.Plots
{
    using YAMP;

    sealed class SumericsLinePlot3D : Sumerics3DPlot
    {
        #region Fields

        readonly Plot3DValue _plot;

        #endregion

        #region ctor

        public SumericsLinePlot3D(Plot3DValue plot)
            : base(plot)
        {
            _plot = plot;
        }

        #endregion
    }
}
