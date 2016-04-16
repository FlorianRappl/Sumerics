namespace Sumerics.Plots.Models
{
    using System;

    public class GridPlotModel : BasePlotModel
    {
        public Int32 Rows { get; set; }

        public Int32 Columns { get; set; }

        public SubplotModel[] Models { get; set; }

        public String Title { get; set; }
    }
}
