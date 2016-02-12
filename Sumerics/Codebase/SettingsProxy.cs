namespace Sumerics
{
    using Sumerics.Properties;
    using System;

    sealed class SettingsProxy : ISettings
    {
        readonly Settings _settings;

        public event EventHandler Changed;

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
        }

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
            _settings.Save();

            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }
    }
}
