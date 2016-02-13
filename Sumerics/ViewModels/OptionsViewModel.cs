namespace Sumerics.ViewModels
{
    using Sumerics.Models;
    using Sumerics.Views;
    using System;
    using System.IO;
    using System.Security.Principal;
    using System.Windows.Input;

	sealed class OptionsViewModel : BaseViewModel
	{
		#region Fields

        readonly OptionsModel _options;
        readonly ICommand _save;
        readonly ICommand _viewErrorLog;
        readonly ICommand _viewLocalScript;
        readonly ICommand _viewGlobalScript;

		#endregion

		#region ctor

		public OptionsViewModel(ISettings settings, Action<String> loadEditor)
		{
            _options = new OptionsModel
            {
                LiveSensorData = settings.LiveSensorData,
                ConsoleFontSize = settings.ConsoleFontSize,
                LiveSensorHistory = settings.LiveSensorHistory,
                AutoSaveHistory = settings.AutoSaveHistory,
                AutoEvaluate = settings.AutoEvaluate,
                Accelerometer = settings.Accelerometer,
                Compass = settings.Compass,
                Gyrometer = settings.Gyrometer,
                Inclinometer = settings.Inclinometer,
                Light = settings.Light
            };
            _save = new RelayCommand(x =>
            {
                var window = x as OptionsWindow;
                settings.LiveSensorData = _options.LiveSensorData;
                settings.ConsoleFontSize = _options.ConsoleFontSize;
                settings.LiveSensorHistory = _options.LiveSensorHistory;
                settings.AutoSaveHistory = _options.AutoSaveHistory;
                settings.AutoEvaluate = _options.AutoEvaluate;
                settings.Accelerometer = _options.Accelerometer;
                settings.Compass = _options.Compass;
                settings.Gyrometer = _options.Gyrometer;
                settings.Inclinometer = _options.Inclinometer;
                settings.Light = _options.Light;
                settings.Save();

                if (window != null)
                {
                    window.Close();
                }
            });
            _viewErrorLog = new RelayCommand(x => loadEditor(Kernel.ErrorLog));
            _viewLocalScript = new RelayCommand(x => loadEditor(Kernel.LocalScript));
            _viewGlobalScript = new RelayCommand(x => loadEditor(Kernel.GlobalScript));
		}

		#endregion

		#region Properties

        public Boolean CanViewGlobalScript
        {
            get
            {
                var isAdmin = false;

                try
                {
                    //get the currently logged in user
                    var user = WindowsIdentity.GetCurrent();
                    var principal = new WindowsPrincipal(user);
                    isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                catch
                {
                }

                return isAdmin;
            }
        }

        public Boolean LiveSensorData
		{
			get { return _options.LiveSensorData; }
			set
			{
                _options.LiveSensorData = value;
				RaisePropertyChanged();
			}
		}

        public Boolean Accelerometer
		{
            get { return _options.Accelerometer; }
			set
			{
                _options.Accelerometer = value;
				RaisePropertyChanged();
			}
		}

        public Boolean Gyrometer
		{
            get { return _options.Gyrometer; }
			set
			{
                _options.Gyrometer = value;
				RaisePropertyChanged();
			}
		}

        public Boolean Inclinometer
		{
            get { return _options.Inclinometer; }
			set
			{
                _options.Inclinometer = value;
				RaisePropertyChanged();
			}
		}

        public Boolean Compass
		{
            get { return _options.Compass; }
			set
			{
                _options.Compass = value;
				RaisePropertyChanged();
			}
		}

		public Boolean Light
		{
            get { return _options.Light; }
			set
			{
                _options.Light = value;
				RaisePropertyChanged();
			}
		}

		public Boolean AutoSaveHistory
		{
            get { return _options.AutoSaveHistory; }
			set
			{
                _options.AutoSaveHistory = value;
				RaisePropertyChanged();
			}
		}

		public Int32 LiveSensorHistory
		{
            get { return _options.LiveSensorHistory; }
			set
			{
                _options.LiveSensorHistory = Math.Max(Math.Min(value, 300), 5);
				RaisePropertyChanged();
			}
		}

		public Int32 ConsoleFontSize
		{
			get { return _options.ConsoleFontSize;  }
			set
			{
                _options.ConsoleFontSize = Math.Max(Math.Min(value, 32), 10);
				RaisePropertyChanged();
			}
		}

		public Boolean AutoEvaluate
		{
			get { return _options.AutoEvaluate; }
			set
			{
                _options.AutoEvaluate = value;
				RaisePropertyChanged();
			}
		}

        #endregion

        #region Commands

        public ICommand SaveAndClose
		{
			get { return _save; }
		}

        public ICommand ViewErrorLog
        {
            get { return _viewErrorLog; }
        }

        public ICommand ViewLocalScript
        {
            get { return _viewLocalScript; }
        }

        public ICommand ViewGlobalScript
        {
            get { return _viewGlobalScript; }
        }

		#endregion
    }
}
