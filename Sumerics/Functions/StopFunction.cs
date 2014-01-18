using System;
using YAMP;

namespace Sumerics
{
    [Description("Stops all currently active computations.")]
    [Kind("UI")]
    sealed class StopFunction : ArgumentFunction
    {
        public static IFunction Create()
        {
            return new StopFunction();
        }

        [Description("Sumerics performs every computation in a new thread, giving you great flexibility and a smooth UI. Therefore the only way to actively stop a computation is to kill it. This function kills all outstanding threads, which stops the related computations.")]
        public void Function()
        {
            App.Window.StopComputations();
        }
    }
}
