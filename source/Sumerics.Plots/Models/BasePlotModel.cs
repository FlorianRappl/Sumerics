namespace Sumerics.Plots.Models
{
    using System;

    public abstract class BasePlotModel
    {
        public Boolean CanEditSeries { get; set; }

        public Boolean CanToggleGrid { get; set; }
    }
}
