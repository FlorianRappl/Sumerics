namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Managers;
    using Sumerics.MathInput;
    using Sumerics.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Fields

        readonly MainViewModel _vm;
        readonly SensorManager _sensors;

        #endregion

        #region ctor

        public MainWindow(MainViewModel vm)
		{
            InitializeComponent();
            AllowDrop = true;

			Loaded += MainWindowLoaded;
			Closing += MainWindowClosing;
            MyConsole.MathInputReceived += MathInputReceived;
            MainTabs.SelectionChanged += CurrentTabChanged;

            _sensors = new SensorManager(SensorGrid);

            vm.Container.All<ISettings>().ForEach(settings => settings.Changed += (s, ev) => LoadSettings());

            DataContext = _vm = vm;
        }

        #endregion

        #region Events

        async void CurrentTabChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (MainTabs.SelectedIndex == 1 && e.AddedItems.Count == 1 && e.AddedItems[0] is HeaderedContentControl)
            {
                //This "hack" is required due to problems with the MetroTabs...
                //obviously they require like ~100 ms before they can give the
                //Focus away (at least to Windows Forms elements).
                await Task.Delay(100);
                MyConsole.SetFocus();
            }
        }

        void MainWindowClosing(Object sender, System.ComponentModel.CancelEventArgs e)
		{
            var settings = _vm.Container.Get<ISettings>();

            if (settings != null && settings.AutoSaveHistory)
			{
				var history = MyConsole.CommandHistory.ToArray();
                settings.History.Clear();

				foreach (var cmd in history)
				{
                    if (!String.IsNullOrEmpty(cmd))
                    {
                        settings.History.Add(cmd);
                    }
				}

                settings.Save();
			}
		}

        void MainWindowLoaded(Object sender, RoutedEventArgs e)
		{
            var settings = LoadSettings();

            if (settings != null && settings.History != null)
			{
				var index = 0;

                foreach (var cmd in settings.History)
				{
					MyConsole.CommandHistory.Insert(index, cmd);
					index++;
				}
			}

			MyConsole.SetFocus();
		}

        void MathInputReceived(Object sender, String result)
        {
            var service = _vm.Container.Get<IMathInputService>();

            if (service != null)
            {
                var settings = _vm.Container.Get<ISettings>();
                var query = service.ConvertToYamp(result);

                if (settings != null && settings.AutoEvaluate)
                {
                    MyConsole.InsertAndRun(query);
                }
                else
                {
                    MyConsole.Input = query;
                }
            }
        }

        #endregion

        #region Methods

		ISettings LoadSettings()
		{
            var settings = _vm.Container.Get<ISettings>();

            _sensors.InstallAccelerometer(AccelerometerPlot);
            _sensors.InstallGyrometer(GyrometerPlot);
            _sensors.InstallInclinometer(InclinometerPlot);
            _sensors.InstallAmbientLight(LightPlot);
            _sensors.InstallCompass(CompassPlot);

            if (settings == null)
            {
                _sensors.Set(AccelerometerPlot, true, 30);
                _sensors.Set(GyrometerPlot, true, 30);
                _sensors.Set(InclinometerPlot, true, 30);
                _sensors.Set(LightPlot, true, 30);
                _sensors.Set(CompassPlot, true, 30);
                MyConsole.ConsoleFontSize = 16f;
            }
            else
            {
                _sensors.Set(AccelerometerPlot, settings.Accelerometer, settings.LiveSensorHistory);
                _sensors.Set(GyrometerPlot, settings.Gyrometer, settings.LiveSensorHistory);
                _sensors.Set(InclinometerPlot, settings.Inclinometer, settings.LiveSensorHistory);
                _sensors.Set(LightPlot, settings.Light, settings.LiveSensorHistory);
                _sensors.Set(CompassPlot, settings.Compass, settings.LiveSensorHistory);
                MyConsole.ConsoleFontSize = settings.ConsoleFontSize;

                if (Kernel.IsWindows8)
                {
                    SensorsTab.Visibility = Visibility.Visible;

                    if (settings.LiveSensorData && !_sensors.IsRunning)
                    {
                        _sensors.Measure();
                    }
                    else if (!settings.LiveSensorData && _sensors.IsRunning)
                    {
                        _sensors.Cancel();
                    }
                }
                else
                {
                    SensorsTab.Visibility = Visibility.Collapsed;
                }
            }

            return settings;
		}

		void OptionsClick(Object sender, RoutedEventArgs e)
		{
            _vm.Container.Get<IDialogManager>().Open(Dialog.Options);
		}

        void AboutClick(Object sender, RoutedEventArgs e)
        {
            _vm.Container.Get<IDialogManager>().Open(Dialog.About);
		}

        #endregion

        #region Drag and Drop

        protected override void OnDrop(DragEventArgs e)
        {
            var files = GetFileFromDrop(e.Data).ToArray();
            _vm.OpenEditor(files);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            if (GetFileFromDrop(e.Data).Any())
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        static IEnumerable<String> GetFileFromDrop(IDataObject dataObject)
        {
            if (dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                var files = dataObject.GetData(DataFormats.FileDrop) as String[];

                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        var extension = Path.GetExtension(file);

                        if (extension.Equals(".sws", StringComparison.OrdinalIgnoreCase))
                        {
                            yield return file;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
