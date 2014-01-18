using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics.Controls
{
    public class SelectingEventArgs : EventArgs
    {
        public AutocompleteItem Item { get; internal set; }

        public bool Cancel { get; set; }

        public int SelectedIndex { get; set; }

        public bool Handled { get; set; }
    }
}
