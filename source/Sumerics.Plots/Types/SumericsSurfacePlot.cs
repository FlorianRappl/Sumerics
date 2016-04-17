namespace Sumerics.Plots
{
    using YAMP;

    sealed class SumericsSurfacePlot : Sumerics3DPlot
    {
        #region Fields

        readonly SurfacePlotValue _plot;

        #endregion

        #region ctor

        public SumericsSurfacePlot(SurfacePlotValue plot)
            : base(plot)
        {
            _plot = plot;
            //_control.CreateSurface(plot.Nx, plot.Ny);
            UpdateSeries();
            UpdateProperties();
        }

        #endregion

        #region Methods

        protected override void UpdateSeries()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                //_control.SetVertex(i, (Single)_plot[0, i].X, (Single)_plot[0, i].Y, (Single)_plot[0, i].Z);
            }
        }

        protected override void UpdateProperties()
        {
            _model.Title = _plot.Title;
            _model.Color = _plot.Color.OxyColorFromString();
            _model.IsAxisShown = _plot.Gridlines;
            //_control.SetView((Single)_plot.MinX, (Single)_plot.MaxX, (Single)_plot.MinY, (Single)_plot.MaxY, (Single)_plot.MinZ, (Single)_plot.MaxZ);
            //_control.SetColors((Single)_plot.MinZ, (Single)_plot.MaxZ, GenerateColors(_plot.ColorPalette, 20));

            //_control.Publish();

            //_control.SetWireframe(_plot.IsMesh);
            //_control.SetSurface(_plot.IsSurf);
        }

        #endregion
    }
}
