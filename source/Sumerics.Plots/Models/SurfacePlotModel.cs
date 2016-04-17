namespace Sumerics.Plots.Models
{
    using OxyPlot;
    using System;

    public class SurfacePlotModel
    {
        public Plot3dAxis XAxis { get; set; }

        public Plot3dAxis YAxis { get; set; }

        public Plot3dAxis ZAxis { get; set; }

        public Int32 Nx { get; set; }

        public Int32 Ny { get; set; }

        public Boolean IsWireframeShown { get; set; }

        public Boolean IsSurfaceShown { get; set; }

        public SeriesModel Data { get; set; }

        public OxyColor[] Colors { get; set; }
    }
}
