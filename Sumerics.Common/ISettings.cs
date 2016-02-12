namespace Sumerics
{
    using System;

    public interface ISettings
    {
        Boolean LiveSensorData { get; set; }

        Int32 ConsoleFontSize { get; set; }

        Int32 LiveSensorHistory { get; set; }

        Boolean AutoSaveHistory { get; set; }

        Boolean AutoEvaluate { get; set; }

        Boolean Accelerometer { get; set; }

        Boolean Gyrometer { get; set; }

        Boolean Inclinometer { get; set; }

        Boolean Compass { get; set; }

        Boolean Light { get; set; }

        void Save();

        event EventHandler Changed;
    }
}
