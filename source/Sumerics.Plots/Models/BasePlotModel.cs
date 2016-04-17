namespace Sumerics.Plots.Models
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class BasePlotModel : INotifyPropertyChanged
    {
        Boolean _toggleGrid;
        Boolean _editSeries;

        public event PropertyChangedEventHandler PropertyChanged;

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

        protected void RaisePropertyChanged([CallerMemberName] String propertyName = null)
        {
            var propertyChanged = PropertyChanged;

            if (propertyChanged != null)
            {
                var eventArgs = new PropertyChangedEventArgs(propertyName);
                propertyChanged.Invoke(this, eventArgs);
            }
        }
    }
}
