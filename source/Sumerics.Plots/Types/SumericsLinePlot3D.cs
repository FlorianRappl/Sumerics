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
            UpdateSeries();
            UpdateProperties();
        }

        #endregion

        #region Methods

        protected override void UpdateSeries()
        {
            //_control.SetWireframe(false);
            _model.XAxis.IsLogarithmic = _plot.IsLogX;
            _model.YAxis.IsLogarithmic = _plot.IsLogY;
            _model.ZAxis.IsLogarithmic = _plot.IsLogZ;

            for (var i = 0; i < _plot.Count; i++)
            {
                var series = _plot[i];
                _model.Color = series.Color.OxyColorFromString();
                _model.Thickness = series.LineWidth;

                //var xt = series.Select(m => m.X);

                //if (_isLogx)
                //{
                //    xt = xt.Select(m => Math.Log(m));
                //}

                //var x = xt.ToArray();

                //var yt = series.Select(m => m.Y);

                //if (_isLogy)
                //{
                //    yt = yt.Select(m => Math.Log(m));
                //}

                //var y = yt.ToArray();

                //var zt = series.Select(m => m.Z);

                //if (_isLogz)
                //{
                //    zt = zt.Select(m => Math.Log(m));
                //}

                //var z = zt.ToArray();

                //for (var j = 2; j < series.Count; j += 2)
                //{
                //    var x1 = x[j - 2];
                //    var x2 = x[j - 1];
                //    var x3 = x[j - 0];
                //    var y1 = y[j - 2];
                //    var y2 = y[j - 1];
                //    var y3 = y[j - 0];
                //    var z1 = z[j - 2];
                //    var z2 = z[j - 1];
                //    var z3 = z[j - 0];
                //    _control.AddWireframeVertex(x1, y1, z1, x2, y2, z2, x3, y3, z3);
                //}
            }

            //_control.SetTransformWireframe(_plot.MinX, _plot.MaxX, _plot.MinY, _plot.MaxY, _plot.MinZ, _plot.MaxZ);
        }

        protected override void UpdateProperties()
        {
            _model.Title = _plot.Title;
            _model.IsAxisShown = _plot.Gridlines;
        }

        #endregion
    }
}
