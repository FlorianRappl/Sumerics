using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sumerics
{
	class OptionsViewModel : BaseViewModel
	{
		#region Members

		bool liveSensorData;
		int consoleFontSize;
		int liveSensorHistory;
		bool autoSaveHistory;
		bool autoEvaluate;
		bool accelerometer;
		bool gyrometer;
		bool inclinometer;
		bool compass;
		bool light;

		#endregion

		#region ctor

		public OptionsViewModel()
		{
			var settings = Properties.Settings.Default;
			liveSensorData = settings.LivePlotActive;
			consoleFontSize = settings.ConsoleFontSize;
			liveSensorHistory = settings.LivePlotHistory;
			autoSaveHistory = settings.AutoSaveHistory;
			autoEvaluate = settings.AutoEvaluateMIP;
			accelerometer = settings.Accelerometer;
			compass = settings.Compass;
			gyrometer = settings.Gyrometer;
			inclinometer = settings.Inclinometer;
			light = settings.Light;
		}

		#endregion

		#region Properties

        public bool CanViewGlobalScript
        {
            get
            {
                bool isAdmin;

                try
                {
                    //get the currently logged in user
                    var user = System.Security.Principal.WindowsIdentity.GetCurrent();
                    var principal = new System.Security.Principal.WindowsPrincipal(user);
                    isAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
                }
                catch (Exception)
                {
                    isAdmin = false;
                }

                return isAdmin;
            }
        }

		public bool LiveSensorData
		{
			get { return liveSensorData; }
			set
			{
				liveSensorData = value;
				RaisePropertyChanged();
			}
		}

		public bool Accelerometer
		{
			get { return accelerometer; }
			set
			{
				accelerometer = value;
				RaisePropertyChanged();
			}
		}

		public bool Gyrometer
		{
			get { return gyrometer; }
			set
			{
				gyrometer = value;
				RaisePropertyChanged();
			}
		}

		public bool Inclinometer
		{
			get { return inclinometer; }
			set
			{
				inclinometer = value;
				RaisePropertyChanged();
			}
		}

		public bool Compass
		{
			get { return compass; }
			set
			{
				compass = value;
				RaisePropertyChanged();
			}
		}

		public bool Light
		{
			get { return light; }
			set
			{
				light = value;
				RaisePropertyChanged();
			}
		}

		public bool AutoSaveHistory
		{
			get { return autoSaveHistory; }
			set
			{
				autoSaveHistory = value;
				RaisePropertyChanged();
			}
		}

		public int LiveSensorHistory
		{
			get { return liveSensorHistory; }
			set
			{
				if (value < 5)
					liveSensorHistory = 5;
				else if (value > 300)
					liveSensorHistory = 300;
				else
					liveSensorHistory = value;

				RaisePropertyChanged();
			}
		}

		public int ConsoleFontSize
		{
			get { return consoleFontSize;  }
			set
			{
				if (value < 10)
					consoleFontSize = 10;
				else if (value > 32)
					consoleFontSize = 32;
				else
					consoleFontSize = value;

				RaisePropertyChanged();
			}
		}

		public bool AutoEvaluate
		{
			get { return autoEvaluate; }
			set
			{
				autoEvaluate = value;
				RaisePropertyChanged();
			}
		}

        #endregion

        #region Commands

        public ICommand SaveAndClose
		{
			get
			{
				return new RelayCommand(x =>
				{
					var window = x as OptionsWindow;

					var settings = Properties.Settings.Default;
					settings.ConsoleFontSize = ConsoleFontSize;
					settings.LivePlotActive = LiveSensorData;
					settings.LivePlotHistory = LiveSensorHistory;
					settings.AutoSaveHistory = AutoSaveHistory;
					settings.AutoEvaluateMIP = AutoEvaluate;
					settings.Accelerometer = accelerometer;
					settings.Compass = compass;
					settings.Gyrometer = gyrometer;
					settings.Inclinometer = inclinometer;
					settings.Light = light;
					settings.Save();

					window.Close();
					App.Window.LoadSettings();
				});
			}
		}

        public ICommand ViewErrorLog
        {
            get
            {
                return new RelayCommand(x =>
                {
                    LoadEditor(Core.ErrorLog);
                });
            }
        }

        public ICommand ViewLocalScript
        {
            get
            {
                return new RelayCommand(x =>
                {
                    LoadEditor(Core.LocalScript);
                });
            }
        }

        public ICommand ViewGlobalScript
        {
            get
            {
                return new RelayCommand(x =>
                {
                    LoadEditor(Core.GlobalScript);
                });
            }
        }

		#endregion

        #region Methods

        void LoadEditor(string file)
        {
            if (!System.IO.File.Exists(file))
            {
                var dir = System.IO.Path.GetDirectoryName(file);

                try
                {
                    if (!System.IO.Directory.Exists(dir))
                        System.IO.Directory.CreateDirectory(dir);

                    System.IO.File.Create(file).Close();
                }
                catch
                {
                    OutputDialog.Show("Unexpected error",
                        "The file " + file + " does not exist and could not be created. Usually this is due to unsufficients privileges. Try running Sumerics in admin mode or create the file on your own to get rid of this exception.");
                    return;
                }
            }

            var editor = StaticHelpers.GetWindow<EditorWindow>();
            editor.OpenFile(file);
        }

        #endregion
    }
}
