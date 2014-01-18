using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Gives access to a stopwatch with milliseconds precision.")]
	[Kind(KindAttribute.FunctionKind.System)]
    sealed class TimerFunction : YFunction
	{
        [Description("Starts a new timer and returns the Timer object to control the stopwatch.")]
        [Example("a = timer(); sleep(1000); a.stop()", "Gets a timer, performs an operation and stops the timer.")]
		public Timer Invoke()
		{
            return new Timer();
		}

        public class Timer
        {
            Int64 _startMilliSecs;
            Int64 _elapsedMilliSecs;
            Boolean _isRunning;

            public Timer()
            {
                _elapsedMilliSecs = 0;
                _startMilliSecs = Environment.TickCount;
                _isRunning = true;
            }

            [Description("Outputs the current value of the stopwatch in milliseconds.")]
            public Int64 time
            {
                get { return _isRunning ? Environment.TickCount - _startMilliSecs + _elapsedMilliSecs : _elapsedMilliSecs; }
            }

            [Description("Starts the stopwatch.")]
            public Timer start()
            {
                if (!_isRunning)
                {
                    _startMilliSecs = Environment.TickCount;
                    _isRunning = true;
                }

                return this;
            }

            [Description("Stops the stopwatch.")]
            public Timer stop()
            {
                if (_isRunning)
                {
                    _elapsedMilliSecs += Environment.TickCount - _startMilliSecs;
                    _isRunning = false;
                }

                return this;
            }

            [Description("Resets the stopwatch.")]
            public Timer reset()
            {
                _elapsedMilliSecs = 0;
                _startMilliSecs = Environment.TickCount;
                return this;
            }

            public override string ToString()
            {
                return string.Format("{0} ms", time);
            }
        }
	}
}
