using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;

namespace Sumerics.Controls
{
    class SumericsSurfacePlot : SumericsPlot3D
    {
        #region Members

        SurfacePlotValue plot;

        #endregion

        #region ctor

        public SumericsSurfacePlot(SurfacePlotValue plot)
            : base(plot)
        {
            this.plot = plot;
            control.CreateSurface(plot.Nx, plot.Ny);
            RefreshData();
        }

        #endregion

        #region Properties

        public override bool IsSeriesEnabled
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Methods

        public override void RefreshData()
        {
            for (var i = 0; i < plot.Count; i++)
                control.SetVertex(i, (float)plot[0, i].X, (float)plot[0, i].Y, (float)plot[0, i].Z);

            RefreshProperties();
        }

        public override void RefreshSeries()
        {
            RefreshProperties();
        }

        public override void RefreshProperties()
        {
            control.Title = plot.Title;
            control.WireframeColor = plot[0].Color.ColorFromString();
            control.SetView((float)plot.MinX, (float)plot.MaxX, (float)plot.MinY, (float)plot.MaxY, (float)plot.MinZ, (float)plot.MaxZ);
            control.SetColors((float)plot.MinZ, (float)plot.MaxZ, GenerateColors(plot.ColorPalette, 20));

            control.Publish();

            control.SetWireframe(plot.IsMesh);
            control.SetSurface(plot.IsSurf);
            control.ShowAxis = plot.Gridlines;
        }

        #endregion
    }
}
