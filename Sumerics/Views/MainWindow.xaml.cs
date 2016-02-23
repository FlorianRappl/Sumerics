namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Managers;
    using Sumerics.MathInput;
    using Sumerics.ViewModels;
    using System;
    using System.Diagnostics;
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

            _sensors = new SensorManager();

            vm.Container.All<ISettings>().ForEach(settings => settings.Changed += (s, ev) => LoadSettings());

            DataContext = _vm = vm;
        }

        #endregion

        #region Events

        async void CurrentTabChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1 && e.AddedItems[0] is HeaderedContentControl)
            {
                if (MainTabs.SelectedIndex == 1)
                {
                    //This "hack" is required due to problems with the MetroTabs...
                    //obviously they require like ~100 ms before they can give the
                    //Focus away (at least to Windows Forms elements).
                    await Task.Delay(100);
                    MyConsole.SetFocus();
                }
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
                Debug.WriteLine(result);
                var query = service.ConvertToYamp(result);
                Debug.WriteLine(query);

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

        /// <summary>
        /// (Re-)Loads the settings.
        /// </summary>
		ISettings LoadSettings()
		{
            var settings = _vm.Container.Get<ISettings>();

            _sensors.Install(AccelerometerPlot, SensorGrid);
            _sensors.Install(GyrometerPlot, SensorGrid);
            _sensors.Install(InclinometerPlot, SensorGrid);
            _sensors.Install(LightPlot, SensorGrid);
            _sensors.Install(CompassPlot, SensorGrid);

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

                //if (Core.IsWindows8)
                //{
                //    SensorsTab.Visibility = System.Windows.Visibility.Visible;

                //    if (settings.LivePlotActive && !sensorRunning)
                //    {
                //        PerformMeasurement().FireAndForget();
                //    }
                //    else if (!settings.LivePlotActive && sensorRunning)
                //    {
                //        sensorRunning = false;
                //    }
                //}
                //else
                //{
                //    SensorsTab.Visibility = System.Windows.Visibility.Collapsed;
                //}
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
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as String[];

                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        //TODO if *.sws try open; otherwise open script file
                        Debug.WriteLine(file);
                    }
                }
            }
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        #endregion
    }
}
