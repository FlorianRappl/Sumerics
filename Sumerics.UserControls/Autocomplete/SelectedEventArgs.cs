using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics.Controls
{
    public class SelectedEventArgs : EventArgs
    {
        public AutocompleteItem Item { get; internal set; }
        public FastColoredTextBox Tb { get; set; }
    }
}
