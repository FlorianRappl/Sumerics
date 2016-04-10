namespace Sumerics.Proxies
{
    using Sumerics.Properties;
    using System;
    using System.Collections.Specialized;

    sealed class SettingsProxy : ISettings
    {
        #region Fields

        readonly Settings _settings;

        #endregion

        #region Events

        public event EventHandler Changed;

        #endregion

        #region ctor

        public SettingsProxy(Settings settings)
        {
            _settings = settings;

            LiveSensorData = settings.LivePlotActive;
            ConsoleFontSize = settings.ConsoleFontSize;
            LiveSensorHistory = settings.LivePlotHistory;
            AutoSaveHistory = settings.AutoSaveHistory;
            AutoEvaluate = settings.AutoEvaluateMIP;
            Accelerometer = settings.Accelerometer;
            Compass = settings.Compass;
            Gyrometer = settings.Gyrometer;
            Inclinometer = settings.Inclinometer;
            Light = settings.Light;
            Language = settings.Language;
            History = settings.History ?? new StringCollection();
        }

        #endregion

        #region Properties

        public Boolean LiveSensorData { get; set; }

        public Int32 ConsoleFontSize { get; set; }

        public Int32 LiveSensorHistory { get; set; }

        public Boolean AutoSaveHistory { get; set; }

        public Boolean AutoEvaluate { get; set; }

        public Boolean Accelerometer { get; set; }

        public Boolean Gyrometer { get; set; }

        public Boolean Inclinometer { get; set; }

        public Boolean Compass { get; set; }

        public Boolean Light { get; set; }

        public String Language { get; set; }

        public StringCollection History { get; private set; }

        #endregion

        #region Methods

        public void Save()
        {
            _settings.ConsoleFontSize = ConsoleFontSize;
            _settings.LivePlotActive = LiveSensorData;
            _settings.LivePlotHistory = LiveSensorHistory;
            _settings.AutoSaveHistory = AutoSaveHistory;
            _settings.AutoEvaluateMIP = AutoEvaluate;
            _settings.Accelerometer = Accelerometer;
            _settings.Compass = Compass;
            _settings.Gyrometer = Gyrometer;
            _settings.Inclinometer = Inclinometer;
            _settings.Light = Light;
            _settings.History = History;
            _settings.Language = Language;
            _settings.Save();

            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
