namespace FastColoredTextBoxNS
{
    using System;

    public class RegionChangedEventArgs : EventArgs
    {
        public RegionChangedEventArgs(String oldText, String newText)
        {
            OldText = oldText;
            NewText = newText;
        }

        public String OldText 
        { 
            get; 
            private set; 
        }

        public String NewText 
        { 
            get;
            private set; 
        }
    }
}
