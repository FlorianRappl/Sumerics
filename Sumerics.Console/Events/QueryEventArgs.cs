namespace FastColoredTextBoxNS
{
    using System;

    public class QueryEventArgs : EventArgs
    {
        public OutputRegion Region { get; private set; }

        public String Query { get; private set; }

        public Boolean IsHistoryEntry { get; private set; }

        public QueryEventArgs(String query, OutputRegion region, Boolean historyEntry = true)
        {
            Region = region;
            Query = query;
            IsHistoryEntry = historyEntry;
        }
    }
}
