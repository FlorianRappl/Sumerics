namespace Sumerics.Controls
{
    using FastColoredTextBoxNS;
    using System;

    public sealed class SelectedEventArgs : EventArgs
    {
        public SelectedEventArgs(AutocompleteItem item, FastColoredTextBox tb)
        {
            Item = item;
            Tb = tb;
        }

        public AutocompleteItem Item 
        { 
            get; 
            private set; 
        }

        public FastColoredTextBox Tb 
        { 
            get; 
            private set; 
        }
    }
}
