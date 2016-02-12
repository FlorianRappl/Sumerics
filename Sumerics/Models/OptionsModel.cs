namespace Sumerics.Models
{
    using System;

    sealed class OptionsModel
    {
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
    }
}
