namespace FastColoredTextBoxNS
{
    using System;

    /// <summary>
    /// Clear selected text
    /// </summary>
    internal class ClearSelectedCommand : UndoableCommand
    {
        String _deletedText;

        /// <summary>
        /// Construstor
        /// </summary>
        /// <param name="tb">Underlaying textbox</param>
        public ClearSelectedCommand(TextSource ts)
            : base(ts)
        {
        }

        /// <summary>
        /// Undo operation
        /// </summary>
        public override void Undo()
        {
            ts.CurrentTB.Selection.Start = new Place(sel.FromX, Math.Min(sel.Start.iLine, sel.End.iLine));
            ts.OnTextChanging();
            InsertTextCommand.InsertText(_deletedText, ts);
            ts.OnTextChanged(sel.Start.iLine, sel.End.iLine);
            ts.CurrentTB.Selection.Start = sel.Start;
            ts.CurrentTB.Selection.End = sel.End;
        }

        /// <summary>
        /// Execute operation
        /// </summary>
        public override void Execute()
        {
            var tb = ts.CurrentTB;
            var temp = default(String);
            ts.OnTextChanging(ref temp);

            if (temp == "")
            {
                throw new ArgumentOutOfRangeException();
            }

            _deletedText = tb.Selection.Text;
            ClearSelected(ts);
            lastSel = new RangeInfo(tb.Selection);
            ts.OnTextChanged(lastSel.Start.iLine, lastSel.Start.iLine);
        }

        internal static void ClearSelected(TextSource ts)
        {
            var tb = ts.CurrentTB;
            var start = tb.Selection.Start;
            var end = tb.Selection.End;
            var fromLine = Math.Min(end.iLine, start.iLine);
            var toLine = Math.Max(end.iLine, start.iLine);
            var fromChar = tb.Selection.FromX;
            var toChar = tb.Selection.ToX;

            if (fromLine >= 0)
            {
                if (fromLine == toLine)
                {
                    ts[fromLine].RemoveRange(fromChar, toChar - fromChar);
                }
                else
                {
                    ts[fromLine].RemoveRange(fromChar, ts[fromLine].Count - fromChar);
                    ts[toLine].RemoveRange(0, toChar);
                    ts.RemoveLine(fromLine + 1, toLine - fromLine - 1);
                    InsertCharCommand.MergeLines(fromLine, ts);
                }

                tb.Selection.Start = new Place(fromChar, fromLine);
                ts.NeedRecalc(new TextSource.TextChangedEventArgs(fromLine, toLine));
            }
        }

        public override UndoableCommand Clone()
        {
            return new ClearSelectedCommand(ts);
        }
    }
}
