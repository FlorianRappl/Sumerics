namespace Sumerics.ViewModels
{
    using System;
    using YAMP.Sensors.Devices;

    sealed class AboutViewModel : BaseViewModel, IDisposable
    {
        readonly SumericsInfo _info;
        readonly Gps _gps;
        readonly Compass _compass;
        readonly ClientPosition _client;

        public AboutViewModel()
        {
            _info = SumericsInfo.FromCurrentAssembly();
            _gps = new Gps();
            _compass = new Compass();
            _client = new ClientPosition
            {
                Direction = _compass.CurrentHeading.Magnetic,
                Latitude = _gps.CurrentLocation.Latitude,
                Longitude = _gps.CurrentLocation.Longitude
            };

            _gps.Changed += PositionChanged;
            _compass.Changed += DirectionChanged;
        }

        void DirectionChanged(Object sender, CompassEventArgs e)
        {
            _client.Direction = e.Value.Magnetic;
            RaisePropertyChanged("Position");
        }

        void PositionChanged(Object sender, GpsEventArgs e)
        {
            _client.Longitude = e.Value.Longitude;
            _client.Latitude = e.Value.Latitude;
            RaisePropertyChanged("Position");
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
            get { return _client.ToString(); }
        }

        void IDisposable.Dispose()
        {
            _gps.Changed -= PositionChanged;
            _compass.Changed -= DirectionChanged;
        }

        sealed class ClientPosition
        {
            public Double Longitude;
            public Double Latitude;
            public Double Direction;

            public override String ToString()
            {
                var message = Messages.AboutPositionMessage;
                return String.Format(message, Longitude.ToString("0"), Latitude.ToString("0"), Direction.ToString("0"));
            }
        }
    }
}
