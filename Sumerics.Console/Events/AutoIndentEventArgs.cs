using System;
using System.Collections.Generic;
using System.Text;

namespace FastColoredTextBoxNS
{
    public class AutoIndentEventArgs : EventArgs
    {
        public AutoIndentEventArgs(int iLine, string lineText, string prevLineText, int tabLength)
        {
            this.iLine = iLine;
            LineText = lineText;
            PrevLineText = prevLineText;
            TabLength = tabLength;
        }

        public int iLine { get; internal set; }
        public int TabLength { get; internal set; }
        public string LineText { get; internal set; }
        public string PrevLineText { get; internal set; }

        /// <summary>
        /// Additional spaces count for this line, relative to previous line
        /// </summary>
        public int Shift { get; set; }

        /// <summary>
        /// Additional spaces count for next line, relative to previous line
        /// </summary>
        public int ShiftNextLines { get; set; }
    }
}
