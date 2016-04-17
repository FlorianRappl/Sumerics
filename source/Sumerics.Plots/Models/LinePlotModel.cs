namespace Sumerics.Plots.Models
{
    public class LinePlotModel
    {
        public Plot3dAxis XAxes { get; set; }

        public Plot3dAxis YAxes { get; set; }

        public Plot3dAxis ZAxes { get; set; }

        public SeriesModel[] Series { get; set; }
    }
}
