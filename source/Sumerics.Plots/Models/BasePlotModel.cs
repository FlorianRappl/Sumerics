namespace Sumerics.Plots.Models
{
    using System;

    public abstract class BasePlotModel : LiveModel
    {
        Boolean _toggleGrid;
        Boolean _editSeries;

        public Boolean CanEditSeries
        {
            get { return _editSeries; }
            set { _editSeries = value; RaisePropertyChanged(); }
        }

        public Boolean CanToggleGrid
        {
            get { return _toggleGrid; }
            set { _toggleGrid = value; RaisePropertyChanged(); }
        }
    }
}
