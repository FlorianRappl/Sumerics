namespace FastColoredTextBoxNS
{
    using System;
    using System.Drawing;

    public class OutputRegion
    {
        #region Fields

        String _text;

        #endregion

        #region Events

        public event EventHandler<RegionChangedEventArgs> TextChanged;

        #endregion

        #region ctor

        internal OutputRegion(Int32 startLine)
        {
            Style = new TextStyle(Brushes.LightGray, Brushes.Transparent, FontStyle.Regular);
            StartLine = startLine;
            Lines = 0;
        }

        #endregion

        #region Properties

        public Boolean Fold 
        {
            get; 
            set; 
        }

        public Int32 StartLine 
        {
            get; 
            private set; 
        }

        public Int32 Lines
        {
            get;
            internal set; 
        }

        public TextStyle Style 
        {
            get;
            set; 
        }

        public String Text
        {
            get { return _text; }
            set 
            {
                if (String.CompareOrdinal(_text, value) != 0)
                {
                    var old = _text;
                    _text = value;

                    if (TextChanged != null)
                    {
                        var ev = new RegionChangedEventArgs(old, value);
                        TextChanged(this, ev);
                    }
                }
            }
        }

        #endregion
    }
}
