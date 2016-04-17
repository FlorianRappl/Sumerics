namespace Sumerics.Plots
{
    using Sumerics.Plots.Models;
    using System;
    using System.Linq;
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
            UpdateSeries();
            UpdateProperties();
        }

        #endregion

        #region Methods

        protected override void UpdateSeries()
        {
            var logx = _plot.IsLogX;
            var logy = _plot.IsLogY;
            var logz = _plot.IsLogZ;
            var modelSeries = new SeriesModel[_plot.Count];

            for (var i = 0; i < _plot.Count; i++)
            {
                var series = _plot[i];
                var xt = series.Select(m => m.X);
                var yt = series.Select(m => m.Y);
                var zt = series.Select(m => m.Z);

                if (logx)
                {
                    xt = xt.Select(m => Math.Log(m));
                }

                if (logy)
                {
                    yt = yt.Select(m => Math.Log(m));
                }

                if (logz)
                {
                    zt = zt.Select(m => Math.Log(m));
                }

                modelSeries[i] = new SeriesModel
                {
                    Xs = xt.ToArray(),
                    Ys = yt.ToArray(),
                    Zs = zt.ToArray(),
                    Color = series.Color.OxyColorFromString(),
                    Thickness = series.LineWidth
                };
            }

            _model.Model = new LinePlotModel
            {
                XAxes = new Plot3dAxis
                {
                    IsLogarithmic = logx,
                    Minimum = _plot.MinX,
                    Maximum = _plot.MaxX
                },
                YAxes = new Plot3dAxis
                {
                    IsLogarithmic = logy,
                    Minimum = _plot.MinY,
                    Maximum = _plot.MaxY
                },
                ZAxes = new Plot3dAxis
                {
                    IsLogarithmic = logz,
                    Minimum = _plot.MinZ,
                    Maximum = _plot.MaxZ
                },
                Series = modelSeries
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
