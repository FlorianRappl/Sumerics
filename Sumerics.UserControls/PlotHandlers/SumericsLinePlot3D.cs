using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;

namespace Sumerics.Controls
{
    class SumericsLinePlot3D : SumericsPlot3D
    {
        #region Members

        Plot3DValue plot;

        bool islogx;
        bool islogy;
        bool islogz;

        #endregion

        #region ctor

        public SumericsLinePlot3D(Plot3DValue plot)
            : base(plot)
        {
            this.plot = plot;
            RefreshData();
            RefreshProperties();
        }

        #endregion

        #region Methods

        public override void RefreshData()
        {
            control.SetWireframe(false);
            islogx = plot.IsLogX;
            islogy = plot.IsLogY;
            islogz = plot.IsLogZ;

            for (var i = 0; i < plot.Count; i++)
            {
                var series = plot[i];
                control.WireframeColor = series.Color.ColorFromString();
                control.WireframeThickness = series.LineWidth;

                var xt = series.Select(m => m.X);

                if (islogx)
                    xt = xt.Select(m => Math.Log(m));

                var x = xt.ToArray();

                var yt = series.Select(m => m.Y);

                if (islogy)
                    yt = yt.Select(m => Math.Log(m));

                var y = yt.ToArray();

                var zt = series.Select(m => m.Z);

                if (islogz)
                    zt = zt.Select(m => Math.Log(m));

                var z = zt.ToArray();

                for (var j = 2; j < series.Count; j+=2)
                {
                    var x1 = x[j - 2];
                    var x2 = x[j - 1];
                    var x3 = x[j - 0];
                    var y1 = y[j - 2];
                    var y2 = y[j - 1];
                    var y3 = y[j - 0];
                    var z1 = z[j - 2];
                    var z2 = z[j - 1];
                    var z3 = z[j - 0];
                    control.AddWireframeVertex(x1, y1, z1, x2, y2, z2, x3, y3, z3);
                }
            }

            control.SetTransformWireframe(plot.MinX, plot.MaxX, plot.MinY, plot.MaxY, plot.MinZ, plot.MaxZ);
        }

        public override void RefreshSeries()
        {
            RefreshData();
        }

        public override void RefreshProperties()
        {
            control.Title = plot.Title;
            control.ShowAxis = plot.Gridlines;

            if (islogx != plot.IsLogX || islogy != plot.IsLogY || islogz != plot.IsLogZ)
                RefreshData();
        }

        #endregion
    }
}
