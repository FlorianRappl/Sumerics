using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FastColoredTextBoxNS
{
    public class OutputRegion
    {
        #region Members

        string _text;
        FastColoredTextBox _container;

        #endregion

        #region ctor

        internal OutputRegion(FastColoredTextBox container, int startLine)
        {
            Style = new TextStyle(Brushes.LightGray, Brushes.Transparent, FontStyle.Regular);
            _container = container;
            StartLine = startLine;
            Lines = 0;
        }

        #endregion

        #region Properties

        public bool Fold 
        {
            get; 
            set; 
        }

        public int StartLine 
        {
            get; 
            private set; 
        }

        public int Lines
        {
            get;
            internal set; 
        }

        public TextStyle Style 
        {
            get;
            set; 
        }

		public int CurrentChars
		{
			get 
            {
                return _container.Width / _container.CharWidth; 
            }
		}

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                _container.OnOutputChanged(this);
            }
        }

        #endregion
    }
}
