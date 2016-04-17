namespace Sumerics.Plots.Models
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class LiveModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
