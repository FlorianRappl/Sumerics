using System;
using System.Collections.Generic;
using System.Text;

namespace FastColoredTextBoxNS
{
    /// <summary>
    /// Insert text into given ranges
    /// </summary>
    internal class ReplaceTextCommand : UndoableCommand
    {
        string insertedText;
        List<Range> ranges;
        List<string> prevText = new List<string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tb">Underlaying textbox</param>
        /// <param name="ranges">List of ranges for replace</param>
        /// <param name="insertedText">Text for inserting</param>
        public ReplaceTextCommand(TextSource ts, List<Range> ranges, string insertedText)
            : base(ts)
        {
            //sort ranges by place
            ranges.Sort((r1, r2) =>
            {
                if (r1.Start.iLine == r2.Start.iLine)
                    return r1.Start.iChar.CompareTo(r2.Start.iChar);
                return r1.Start.iLine.CompareTo(r2.Start.iLine);
            });
            //
            this.ranges = ranges;
            this.insertedText = insertedText;
            lastSel = sel = new RangeInfo(ts.CurrentTB.Selection);
        }

        /// <summary>
        /// Undo operation
        /// </summary>
        public override void Undo()
        {
            var tb = ts.CurrentTB;

            ts.OnTextChanging();

            tb.Selection.BeginUpdate();
            for (int i = 0; i < ranges.Count; i++)
            {
                tb.Selection.Start = ranges[i].Start;
                for (int j = 0; j < insertedText.Length; j++)
                    tb.Selection.GoRight(true);
                ClearSelectedCommand.ClearSelected(ts);
                InsertTextCommand.InsertText(prevText[prevText.Count - i - 1], ts);
                ts.OnTextChanged(ranges[i].Start.iLine, ranges[i].Start.iLine);
            }
            tb.Selection.EndUpdate();

            ts.NeedRecalc(new TextSource.TextChangedEventArgs(0, 1));
        }

        /// <summary>
        /// Execute operation
        /// </summary>
        public override void Execute()
        {
            var tb = ts.CurrentTB;
            prevText.Clear();

            ts.OnTextChanging(ref insertedText);

            tb.Selection.BeginUpdate();
            for (int i = ranges.Count - 1; i >= 0; i--)
            {
                tb.Selection.Start = ranges[i].Start;
                tb.Selection.End = ranges[i].End;
                prevText.Add(tb.Selection.Text);
                ClearSelectedCommand.ClearSelected(ts);
                InsertTextCommand.InsertText(insertedText, ts);
                ts.OnTextChanged(ranges[i].Start.iLine, ranges[i].End.iLine);
            }
            tb.Selection.EndUpdate();
            ts.NeedRecalc(new TextSource.TextChangedEventArgs(0, 1));

            lastSel = new RangeInfo(tb.Selection);
        }

        public override UndoableCommand Clone()
        {
            return new ReplaceTextCommand(ts, new List<Range>(ranges), insertedText);
        }
    }
}
