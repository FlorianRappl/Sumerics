using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using YAMP;

namespace YAMP.Sensors
{
    public abstract class SensorFunction : ArgumentFunction
    {
        EventHandler<object> readingChanged;
        int readingChangedHandlers;

        /// <summary>
        /// Notification that the reading of the sending SensorFunction had changed
        /// to get the new values, regular calls to the Function with the chosen modifiers have to be used
        /// </summary>
        public event EventHandler<object> ReadingChanged
        {
            add
            {
                if (readingChangedHandlers == 0)
                    InstallReadingChangedHandler();

                readingChanged += value;
                readingChangedHandlers++;
            }
            remove
            {
                readingChanged -= value;
                readingChangedHandlers--;

                if (readingChangedHandlers == 0)
                    UninstallReadingChangedHandler();
            }
        }

        protected abstract void InstallReadingChangedHandler();

        protected abstract void UninstallReadingChangedHandler();

        protected void RaiseReadingChanged(object parameter)
        {
            if (readingChanged != null)
                readingChanged(this, parameter);
        }
    }
}
