namespace Sumerics
{
    using MahApps.Metro.Controls;
    //using YAMP.Sensors;
    using Sumerics.Controls;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using YAMP;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Members

		bool sensorRunning;
		bool initial;

        #endregion

        #region ctor

        public MainWindow()
		{
            App.Window = this;
			initial = true;

            InitializeComponent();
            this.AllowDrop = true;

			LoadSettings();
			Loaded += MainWindowLoaded;
			Closing += MainWindowClosing;
            MyConsole.MathInputReceived += MathInputReceived;
            MainTabs.SelectionChanged += CurrentTabChanged;
        }

        #endregion

        #region Events

        async void CurrentTabChanged(object sender, SelectionChangedEventArgs e)
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

        void MainWindowClosing(object sender, CancelEventArgs e)
		{
			var pers = Properties.Settings.Default;

			if (pers != null && pers.AutoSaveHistory)
			{
				var history = MyConsole.CommandHistory.ToArray();
				pers.History = new StringCollection();

				foreach (var cmd in history)
				{
					if (!string.IsNullOrEmpty(cmd))
						pers.History.Add(cmd);
				}

				pers.Save();
			}
		}

		void MainWindowLoaded(object sender, RoutedEventArgs e)
		{
			DataContext = new MainViewModel();
			var pers = Properties.Settings.Default;

			if (pers != null && pers.History != null)
			{
				int index = 0;

				foreach (var cmd in pers.History)
				{
					MyConsole.CommandHistory.Insert(index, cmd);
					index++;
				}
			}

			MyConsole.SetFocus();
		}

        void MathInputReceived(object sender, string result)
        {
            Debug.WriteLine(result);
            var query = MathMLParser.Parse(result);
            Debug.WriteLine(query);

            if (Properties.Settings.Default != null && Properties.Settings.Default.AutoEvaluateMIP)
                MyConsole.InsertAndRun(query);
            else
                MyConsole.Input = query;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Runs a query with the given query string.
        /// </summary>
        /// <param name="query">The query string to evaluate.</param>
		public void RunQuery(string query)
		{
			MyConsole.InsertAndRun(query);
		}

        /// <summary>
        /// Runs a query with the given query string and console message.
        /// </summary>
        /// <param name="query">The query string to evaluate.</param>
        /// <param name="message">The message to display as a comment.</param>
        public void RunQuery(string query, string message)
        {
            MyConsole.InsertAndRun(query, message);
        }

        /// <summary>
        /// (Re-)Loads the settings.
        /// </summary>
		public void LoadSettings()
		{
			var settings = Properties.Settings.Default;

            if (settings == null)
            {
                Set(AccelerometerPlot, true, 30);
                Set(GyrometerPlot, true, 30);
                Set(InclinometerPlot, true, 30);
                Set(LightPlot, true, 30);
                Set(CompassPlot, true, 30);
                MyConsole.ConsoleFontSize = 16f;
                return;
            }

			Set(AccelerometerPlot, settings.Accelerometer, settings.LivePlotHistory);
			Set(GyrometerPlot, settings.Gyrometer, settings.LivePlotHistory);
			Set(InclinometerPlot, settings.Inclinometer, settings.LivePlotHistory);
			Set(LightPlot, settings.Light, settings.LivePlotHistory);
			Set(CompassPlot, settings.Compass, settings.LivePlotHistory);
			MyConsole.ConsoleFontSize = settings.ConsoleFontSize;

            if (Core.IsWindows8)
            {
                SensorsTab.Visibility = System.Windows.Visibility.Visible;

                if (settings.LivePlotActive && !sensorRunning)
                {
                    PerformMeasurement().FireAndForget();
                }
                else if (!settings.LivePlotActive && sensorRunning)
                {
                    sensorRunning = false;
                }
            }
            else
            {
                SensorsTab.Visibility = System.Windows.Visibility.Collapsed;
            }

			initial = false;
		}

		void Set(SensorPlot plot, bool show, int length)
		{
			plot.Length = length;
			plot.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
			SensorGrid.RowDefinitions[Grid.GetRow(plot)].Height = show ? new GridLength(1.0, GridUnitType.Star) : new GridLength(0.0);
			plot.Maximized = false;

			if (initial)
			{
				plot.PreviewMouseDown += (s, e) =>
				{
					var row = Grid.GetRow(plot);

					for (var i = 0; i < SensorGrid.RowDefinitions.Count; i++)
					{
						if (i == row)
							continue;

						var sensor = SensorGrid.Children[i] as SensorPlot;

                        if (sensor == null)
                            return;

						if(sensor.Visibility == System.Windows.Visibility.Visible)
							SensorGrid.RowDefinitions[i].Height = plot.Maximized ? new GridLength(1.0, GridUnitType.Star) : new GridLength(0.0);
					}

					plot.Maximized = !plot.Maximized;
				};
			}
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

		void OptionsClick(object sender, RoutedEventArgs e)
		{
            OpenOptionsWindow();
		}

		void AboutClick(object sender, RoutedEventArgs e)
		{
            OpenAboutWindow();
		}

        /// <summary>
        /// Docks an image to the visualization tab.
        /// </summary>
        /// <param name="plot"></param>
        public void DockImage(IPlotViewModel plot)
        {
            (DataContext as MainViewModel).LastPlot = plot;
        }

        #endregion

        #region Drag and Drop

        protected override void OnDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];

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
                e.Effects = DragDropEffects.Link;
            else
                e.Effects = DragDropEffects.None;
        }

        #endregion

        #region UI Manipulators

        internal void ChangeTab(int index)
        {
            Dispatcher.Invoke(() =>
            {
                if(index >= 0 && index < MainTabs.Items.Count)
                    MainTabs.SelectedIndex = index;
            });
        }

        internal void UndockImage()
        {
            Dispatcher.Invoke(() =>
            {
                MyLastPlot.Undock();
            });
        }

        internal void UndockImage(PlotValue value)
        {
            Dispatcher.Invoke(() =>
            {
                if(MyLastPlot.Data == null || MyLastPlot.Data.Plot != value)
                    new PlotViewModel(value).UndockPlot();
                else
                    MyLastPlot.Undock();
            });
        }

        internal void DockImage()
        {
            Dispatcher.Invoke(() =>
            {
                PlotWindow win = null;

                foreach (var window in App.Current.Windows)
                {
                    if (window is PlotWindow)
                        win = (PlotWindow)window;
                }

                if (win == null)
                    return;

                (DataContext as MainViewModel).LastPlot = win.PlotModel;
                win.Close();
            });
        }

        internal void DockImage(PlotValue value)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var window in App.Current.Windows)
                {
                    if (window is PlotWindow)
                    {
                        var win = (PlotWindow)window;

                        if (win.PlotModel.Plot == value)
                        {
                            DockImage(win.PlotModel);
                            win.Close();
                        }
                    }
                }
            });
        }

        internal void OpenOptionsWindow()
        {
            Dispatcher.Invoke(() =>
            {
                StaticHelpers.GetWindow<OptionsWindow>();
            });
        }

        internal void OpenAboutWindow()
        {
            Dispatcher.Invoke(() =>
            {
                StaticHelpers.GetWindow<AboutWindow>();
            });
        }

        internal void OpenEditorWindow()
        {
            Dispatcher.Invoke(() =>
            {
                StaticHelpers.GetWindow<EditorWindow>();
            });
        }

        internal void OpenDocumentationWindow()
        {
            Dispatcher.Invoke(() =>
            {
                StaticHelpers.GetWindow<DemoBrowser>().Show();
            });
        }

        internal void OpenHelpWindow()
        {
            Dispatcher.Invoke(() =>
            {
                StaticHelpers.GetWindow<HelpWindow>().Show();
            });
        }

        internal void OpenDirectoryWindow()
        {
            Dispatcher.Invoke(() =>
            {
                var dialog = new FolderBrowseWindow();
                dialog.ShowDialog();

                if (dialog.Accepted)
                    Environment.CurrentDirectory = dialog.SelectedDirectory;
            });
        }

        internal void OpenLoadWindow()
        {
            Dispatcher.Invoke(() =>
            {
                var dialog = new OpenFileWindow();
                dialog.Title = "Open workspace ...";
                dialog.AddFilter("All files (*.*)", "*.*");
                dialog.AddFilter("Sumerics workspace (*.sws)", "*.sws");
                dialog.ShowDialog();

                if (dialog.Accepted)
                    Core.LoadWorkspaceAsync(dialog.SelectedFile);
            });
        }

        internal void OpenSaveWindow()
        {
            Dispatcher.Invoke(() =>
            {
                var dialog = new SaveFileWindow();
                dialog.Title = "Save workspace as ...";
                dialog.AddFilter("Sumerics workspace (*.sws)", "*.sws");
                dialog.ShowDialog();

                if (dialog.Accepted)
                    Core.SaveWorkspaceAsync(dialog.SelectedFile);
            });
        }

        async internal void StopComputations()
        {
            await Task.Delay(5);
            await Dispatcher.InvokeAsync(() =>
            {
                if (QueryResultViewModel.HasRunningQueries)
                {
                    foreach (var query in QueryResultViewModel.RunningQueries)
                    {
                        query.Cancel();
                    }
                }
            });
        }

        #endregion
    }
}
