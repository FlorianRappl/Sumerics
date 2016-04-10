namespace Sumerics.Managers
{
    using Sumerics.Controls;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using YAMP.Sensors.Devices;

    sealed class SensorManager
    {
        static readonly GridLength RemainingHeight = new GridLength(1.0, GridUnitType.Star);
        static readonly GridLength ZeroHeight = new GridLength(0.0);

        readonly Accelerometer _accelerometer;
        readonly Gyrometer _gyrometer;
        readonly Inclinometer _inclinometer;
        readonly AmbientLight _light;
        readonly Compass _compass;
        readonly List<Action> _updaters;
        readonly Grid _grid;
        Task _task;
        CancellationTokenSource _cts;

        public SensorManager(Grid grid)
        {
            _accelerometer = new Accelerometer();
            _gyrometer = new Gyrometer();
            _inclinometer = new Inclinometer();
            _light = new AmbientLight();
            _compass = new Compass();
            _updaters = new List<Action>();
            _grid = grid;
        }

        public Boolean IsRunning
        {
            get { return _task != null; }
        }

        public void Cancel()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
                _task = null;
            }
        }

        public void Measure()
        {
            if (_task == null)
            {
                _cts = new CancellationTokenSource();
                _task = MeasureAsync(_cts.Token);
            }
        }

        public void InstallAccelerometer(SensorPlot plot)
        {
            Install(plot);
            _updaters.Add(() =>
            {
                var acc = _accelerometer.CurrentAcceleration;
                plot.AddValues(acc.X, acc.Y, acc.Z);
            });
        }

        public void InstallGyrometer(SensorPlot plot)
        {
            Install(plot);
            _updaters.Add(() =>
            {
                var gyro = _gyrometer.CurrentAngularVelocity;
                plot.AddValues(gyro.X, gyro.Y, gyro.Z);
            });
        }

        public void InstallInclinometer(SensorPlot plot)
        {
            Install(plot);
            _updaters.Add(() =>
            {
                var inc = _inclinometer.CurrentGradient;
                plot.AddValues(inc.Pitch, inc.Roll, inc.Yaw);
            });
        }

        public void InstallAmbientLight(SensorPlot plot)
        {
            Install(plot);
            _updaters.Add(() =>
            {
                var light = _light.CurrentLight;
                plot.AddValues(light);
            });
        }

        public void InstallCompass(SensorPlot plot)
        {
            Install(plot);
            _updaters.Add(() =>
            {
                var comp = _compass.CurrentHeading;
                plot.AddValues(comp.Magnetic);
            });
        }

        public void Set(SensorPlot plot, Boolean show, Int32 length)
        {
            plot.Length = length;
            plot.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            plot.Maximized = false;
            _grid.RowDefinitions[Grid.GetRow(plot)].Height = show ? RemainingHeight : ZeroHeight;
        }

        void Install(SensorPlot plot)
        {
            plot.PreviewMouseDown += (s, e) =>
            {
                var row = Grid.GetRow(plot);

                for (var i = 0; i < _grid.RowDefinitions.Count; i++)
                {
                    if (i != row)
                    {
                        var sensor = _grid.Children[i] as SensorPlot;

                        if (sensor != null && sensor.Visibility == System.Windows.Visibility.Visible)
                        {
                            var auto = new GridLength(1.0, GridUnitType.Star);
                            var height = plot.Maximized ? auto : new GridLength(0.0);
                            _grid.RowDefinitions[i].Height = height;
                        }
                    }
                }

                plot.Maximized = !plot.Maximized;
            };
        }

        async Task MeasureAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var updater in _updaters)
                {
                    updater.Invoke();
                }

                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
