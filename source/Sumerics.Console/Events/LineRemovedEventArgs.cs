using System;
using System.Collections.Generic;
using System.Text;

namespace FastColoredTextBoxNS
{
    public class LineRemovedEventArgs : EventArgs
    {
        public LineRemovedEventArgs(int index, int count, List<int> removedLineIds)
        {
            Index = index;
            Count = count;
            RemovedLineUniqueIds = removedLineIds;
        }

        /// <summary>
        /// Removed line index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Count of removed lines
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// UniqueIds of removed lines
        /// </summary>
        public List<int> RemovedLineUniqueIds { get; set; }
    }
}
