namespace Sumerics.Plots.Models
{
    using System;

    public class GridPlotModel
    {
        Int32 Rows { get; set; }

        Int32 Columns { get; set; }

        Object[,] Models { get; set; }

        String Title { get; set; }
    }
}
