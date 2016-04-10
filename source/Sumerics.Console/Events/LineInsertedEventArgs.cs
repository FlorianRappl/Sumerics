using System;
using System.Collections.Generic;
using System.Text;

namespace FastColoredTextBoxNS
{
    public class LineInsertedEventArgs : EventArgs
    {
        public LineInsertedEventArgs(int index, int count)
        {
            Index = index;
            Count = count;
        }

        /// <summary>
        /// Inserted line index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Count of inserted lines
        /// </summary>
        public int Count { get; set; }
    }
}
