namespace Sumerics.Models
{
    using Sumerics.Properties;
    using System;

    sealed class OptionsModel
    {
        public OptionsModel(Settings settings)
        {
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

        public void Save(Settings settings)
        {
            settings.ConsoleFontSize = ConsoleFontSize;
            settings.LivePlotActive = LiveSensorData;
            settings.LivePlotHistory = LiveSensorHistory;
            settings.AutoSaveHistory = AutoSaveHistory;
            settings.AutoEvaluateMIP = AutoEvaluate;
            settings.Accelerometer = Accelerometer;
            settings.Compass = Compass;
            settings.Gyrometer = Gyrometer;
            settings.Inclinometer = Inclinometer;
            settings.Light = Light;
            settings.Save();
        }
    }
}
