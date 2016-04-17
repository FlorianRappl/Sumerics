namespace Sumerics.Plots
{
    using Sumerics.Plots.Models;
    using System;
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
            UpdateSeries();
            UpdateProperties();
        }

        #endregion

        #region Methods

        protected override void UpdateSeries()
        {
            var x = new Double[_plot.Count];
            var y = new Double[_plot.Count];
            var z = new Double[_plot.Count];

            for (var i = 0; i < _plot.Count; i++)
            {
                x[i] = _plot[0, i].X;
                y[i] = _plot[0, i].Y;
                z[i] = _plot[0, i].Z;
            }

            _model.Model = new SurfacePlotModel
            {
                Nx = _plot.Nx,
                Ny = _plot.Ny,
                XAxis = new Plot3dAxis
                {
                    IsLogarithmic = false,
                    Minimum = _plot.MinX,
                    Maximum = _plot.MaxX
                },
                YAxis = new Plot3dAxis
                {
                    IsLogarithmic = false,
                    Minimum = _plot.MinY,
                    Maximum = _plot.MaxY
                },
                ZAxis = new Plot3dAxis
                {
                    IsLogarithmic = false,
                    Minimum = _plot.MinZ,
                    Maximum = _plot.MaxZ
                },
                IsWireframeShown = _plot.IsMesh,
                IsSurfaceShown = _plot.IsSurf,
                Data = new SeriesModel
                {
                    Xs = x,
                    Ys = y,
                    Zs = z,
                    Color = _plot.Color.OxyColorFromString(),
                    Thickness = _plot.MeshThickness
                },
                Colors = _plot.ColorPalette.GenerateColors(20)
            };
        }

        protected override void UpdateProperties()
        {
            _model.Title = _plot.Title;
            _model.IsAxisShown = _plot.Gridlines;
        }

        #endregion
    }
}
