namespace FastColoredTextBoxNS
{
    using System;
    using System.Drawing;

    public class OutputRegion
    {
        #region Fields

        readonly FastColoredTextBox _container;
        String _text;

        #endregion

        #region ctor

        internal OutputRegion(FastColoredTextBox container, Int32 startLine)
        {
            Style = new TextStyle(Brushes.LightGray, Brushes.Transparent, FontStyle.Regular);
            _container = container;
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

        public Int32 CurrentChars
		{
			get { return _container.Width / _container.CharWidth; }
		}

        public String Text
        {
            get { return _text; }
            set 
            {
                _text = value;
                _container.OnOutputChanged(this);
            }
        }

        #endregion
    }
}
