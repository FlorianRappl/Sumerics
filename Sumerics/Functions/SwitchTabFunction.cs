using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;

namespace Sumerics
{
    [Description("Modifies the UI in such a sense that it switches to a specific tab (1-n) of the Sumerics main window.")]
    [Kind("UI")]
    sealed class SwitchTabFunction : ArgumentFunction
    {
        public static IFunction Create()
        {
            return new SwitchTabFunction();
        }

        [Description("Switches to the specified index.")]
        [Example("switchtab(3)", "Opens the visualization tab.")]
        public void Function(ScalarValue index)
        {
            App.Window.ChangeTab(index.IntValue - 1);
        }
    }
}
