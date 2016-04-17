namespace Sumerics.Plots.Models
{
    using OxyPlot;
    using System;

    public class SeriesModel
    {
        public Double[] Xs { get; set; }

        public Double[] Ys { get; set; }

        public Double[] Zs { get; set; }

        public OxyColor Color { get; set; }

        public Double Thickness { get; set; }
    }
}
