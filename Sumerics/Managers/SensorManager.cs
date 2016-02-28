namespace Sumerics.Managers
{
    using Sumerics.Controls;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using YAMP.Sensors.Devices;

    sealed class SensorManager
    {
        readonly Accelerometer _accelerometer;
        readonly Gyrometer _gyrometer;
        readonly Inclinometer _inclinometer;
        readonly AmbientLight _light;
        readonly Compass _compass;
        CancellationTokenSource _cts;

        public SensorManager()
        {
            _accelerometer = new Accelerometer();
            _gyrometer = new Gyrometer();
            _inclinometer = new Inclinometer();
            _light = new AmbientLight();
            _compass = new Compass();
        }

        public void Cancel()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
        }

        public void Install(SensorPlot plot, Grid sensors)
        {
            plot.PreviewMouseDown += (s, e) =>
            {
                var row = Grid.GetRow(plot);

                for (var i = 0; i < sensors.RowDefinitions.Count; i++)
                {
                    if (i != row)
                    {
                        var sensor = sensors.Children[i] as SensorPlot;

                        if (sensor != null && sensor.Visibility == System.Windows.Visibility.Visible)
                        {
                            var auto = new GridLength(1.0, GridUnitType.Star);
                            var height = plot.Maximized ? auto : new GridLength(0.0);
                            sensors.RowDefinitions[i].Height = height;
                        }
                    }
                }

                plot.Maximized = !plot.Maximized;
            };
        }

        public void Set(SensorPlot plot, Boolean show, Int32 length)
        {
            plot.Length = length;
            plot.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            //SensorGrid.RowDefinitions[Grid.GetRow(plot)].Height = show ? new GridLength(1.0, GridUnitType.Star) : new GridLength(0.0);
            plot.Maximized = false;
        }

        public async Task MeasureAsync()
        {
            if (_cts == null)
            {
                _cts = new CancellationTokenSource();

                while (!_cts.IsCancellationRequested)
                {
                    var acc = _accelerometer.CurrentAcceleration;
                    var gyro = _gyrometer.CurrentAngularVelocity;
                    var inc = _inclinometer.CurrentGradient;
                    var light = _light.CurrentLight;
                    var comp = _compass.CurrentHeading;

                    //AccelerometerPlot.AddValues(acc[0], acc[1], acc[2]);
                    //GyrometerPlot.AddValues(gyro[0], gyro[1], gyro[2]);
                    //InclinometerPlot.AddValues(inc[0], inc[1], inc[2]);
                    //LightPlot.AddValues(light);
                    //CompassPlot.AddValues(comp);

                    await Task.Delay(1000, _cts.Token);
                }
            }
        }
    }
}
