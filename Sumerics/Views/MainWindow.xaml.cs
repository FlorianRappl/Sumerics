namespace Sumerics.Views
{
    using MahApps.Metro.Controls;
    using Sumerics.Controls;
    using Sumerics.MathInput;
    using Sumerics.ViewModels;
    using System;
    using System.Collections.Specialized;
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

		Boolean _sensorRunning;
		Boolean _initial;

        #endregion

        #region ctor

        public MainWindow(MainViewModel vm)
		{
			_initial = true;

            InitializeComponent();
            AllowDrop = true;

			Loaded += MainWindowLoaded;
			Closing += MainWindowClosing;
            MyConsole.MathInputReceived += MathInputReceived;
            MainTabs.SelectionChanged += CurrentTabChanged;

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
			var pers = Properties.Settings.Default;

			if (pers != null && pers.AutoSaveHistory)
			{
				var history = MyConsole.CommandHistory.ToArray();
				pers.History = new StringCollection();

				foreach (var cmd in history)
				{
                    if (!String.IsNullOrEmpty(cmd))
                    {
                        pers.History.Add(cmd);
                    }
				}

				pers.Save();
			}
		}

        void MainWindowLoaded(Object sender, RoutedEventArgs e)
		{
            LoadSettings();
			var pers = Properties.Settings.Default;

			if (pers != null && pers.History != null)
			{
				var index = 0;

				foreach (var cmd in pers.History)
				{
					MyConsole.CommandHistory.Insert(index, cmd);
					index++;
				}
			}

			MyConsole.SetFocus();
		}

        void MathInputReceived(Object sender, String result)
        {
            var service = _vm.Container.Get<IMathInput>();

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
		void LoadSettings()
		{
            var settings = _vm.Container.Get<ISettings>();

            if (settings == null)
            {
                //Set(AccelerometerPlot, true, 30);
                //Set(GyrometerPlot, true, 30);
                //Set(InclinometerPlot, true, 30);
                //Set(LightPlot, true, 30);
                //Set(CompassPlot, true, 30);
                MyConsole.ConsoleFontSize = 16f;
                return;
            }

            //Set(AccelerometerPlot, settings.Accelerometer, settings.LivePlotHistory);
            //Set(GyrometerPlot, settings.Gyrometer, settings.LivePlotHistory);
            //Set(InclinometerPlot, settings.Inclinometer, settings.LivePlotHistory);
            //Set(LightPlot, settings.Light, settings.LivePlotHistory);
            //Set(CompassPlot, settings.Compass, settings.LivePlotHistory);
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

			_initial = false;
		}

		void Set(SensorPlot plot, Boolean show, Int32 length)
		{
            //plot.Length = length;
            //plot.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            //SensorGrid.RowDefinitions[Grid.GetRow(plot)].Height = show ? new GridLength(1.0, GridUnitType.Star) : new GridLength(0.0);
            //plot.Maximized = false;

            //if (initial)
            //{
            //    plot.PreviewMouseDown += (s, e) =>
            //    {
            //        var row = Grid.GetRow(plot);

            //        for (var i = 0; i < SensorGrid.RowDefinitions.Count; i++)
            //        {
            //            if (i == row)
            //                continue;

            //            var sensor = SensorGrid.Children[i] as SensorPlot;

            //            if (sensor == null)
            //                return;

            //            if (sensor.Visibility == System.Windows.Visibility.Visible)
            //                SensorGrid.RowDefinitions[i].Height = plot.Maximized ? new GridLength(1.0, GridUnitType.Star) : new GridLength(0.0);
            //        }

            //        plot.Maximized = !plot.Maximized;
            //    };
            //}
		}

        async Task PerformMeasurement()
        {
            //sensorRunning = true;

            //while (sensorRunning)
            //{
            //    var acc = AccFunction.Acceleration;
            //    var gyro = GyroFunction.AngularVelocity;
            //    var inc = IncFunction.Inclination;
            //    var light = LightFunction.Light;
            //    var comp = CompFunction.HeadingMagneticNorth;

            //    AccelerometerPlot.AddValues(acc[0], acc[1], acc[2]);
            //    GyrometerPlot.AddValues(gyro[0], gyro[1], gyro[2]);
            //    InclinometerPlot.AddValues(inc[0], inc[1], inc[2]);
            //    LightPlot.AddValues(light);
            //    CompassPlot.AddValues(comp);

            //    await Task.Delay(1000);
            //}
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
