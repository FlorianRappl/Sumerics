namespace Sumerics.ViewModels
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class BaseViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        readonly IContainer _container;

        public BaseViewModel(IContainer container)
        {
            _container = container;
        }

        public BaseViewModel()
            : this(null)
        {
        }

        public IContainer Container
        {
            get { return _container; }
        }

        protected void RaisePropertyChanged([CallerMemberName] String propertyName = null)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
