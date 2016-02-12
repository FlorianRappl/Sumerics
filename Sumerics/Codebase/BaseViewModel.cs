namespace Sumerics
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class BaseViewModel : INotifyPropertyChanged
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
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
