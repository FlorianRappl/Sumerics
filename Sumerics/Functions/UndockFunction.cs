using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;

namespace Sumerics
{
    [Description("This function is useful if you want to preserve a plot by duplicating it. Duplicating a plot results in a new window with the given plot.")]
    [Kind("UI")]
    sealed class UndockFunction : ArgumentFunction
    {
        public static IFunction Create()
        {
            return new UndockFunction();
        }

        [Description("Undocks the current plot from the main window.")]
        [Example("undock()", "Duplicates the currently selected plot.")]
        public void Function()
        {
            App.Window.UndockImage();
        }

        [Description("Undocks the given plot from the main window.")]
        [Example("undock(cplot(sin))", "Creates a new complex plot and undocks it immediately.")]
        public void Function(PlotValue plot)
        {
            App.Window.UndockImage(plot);
        }
    }
}
