namespace Sumerics.ViewModels
{
    using System;

    sealed class AboutViewModel : BaseViewModel, IDisposable
    {
        static readonly String _message = "Your current position is {0}° {1}° with heading {2}° north.";

        readonly SumericsInfo _info;

        Int32 _longitude;
        Int32 _latitude;
        Int32 _compass;

        //readonly GPSFunction _gps;
        //readonly CompFunction _cmp;

        public AboutViewModel()
        {
            _info = SumericsInfo.FromCurrentAssembly();

            //_gps = new GPSFunction();
            //_gps.ReadingChanged += OnSensorChanged;
            //_cmp = new CompFunction();
            //_cmp.ReadingChanged += OnSensorChanged;
        }

        public String Copyright
        {
            get { return String.Concat(_info.Copyright, ", ", _info.Company); }
        }

        public String Version
        {
            get { return String.Concat(_info.ProductName, ", Version: ", _info.Version); }
        }

        public String Position
        {
            get { return String.Format(_message, _longitude, _latitude, _compass); }
        }

        void OnSensorChanged(Object sender, Object e)
        {
            //_longitude = (int)GPSFunction.Longitude;
            //_latitude = (int)GPSFunction.Latitude;
            //_compass = (int)CompFunction.HeadingMagneticNorth;
            App.Current.Dispatcher.Invoke(() => RaisePropertyChanged("Position"));
        }

        void IDisposable.Dispose()
        {
            //_gps.ReadingChanged -= OnSensorChanged;
            //_cmp.ReadingChanged -= OnSensorChanged;
        }
    }
}
