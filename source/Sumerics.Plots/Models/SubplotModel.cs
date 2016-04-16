namespace Sumerics.Plots.Models
{
    using System;

    public class SubplotModel
    {
        public Int32 RowIndex { get; set; }

        public Int32 ColumnIndex { get; set; }

        public Int32 RowSpan { get; set; }

        public Int32 ColumnSpan { get; set; }

        public IPlotController Controller { get; set; }
    }
}
