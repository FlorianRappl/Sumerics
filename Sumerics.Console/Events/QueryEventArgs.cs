using System;
using System.Collections.Generic;
using System.Text;

namespace FastColoredTextBoxNS
{
    public class QueryEventArgs : EventArgs
    {
        public OutputRegion Region { get; private set; }

        public string Query { get; private set; }

        public bool IsHistoryEntry { get; private set; }

        public QueryEventArgs(string query, OutputRegion region, bool historyEntry = true)
        {
            Region = region;
            Query = query;
            IsHistoryEntry = historyEntry;
        }
    }
}
