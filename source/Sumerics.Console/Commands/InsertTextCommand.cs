using System;
using System.Collections.Generic;
using System.Text;

namespace FastColoredTextBoxNS
{
    /// <summary>
    /// Insert text
    /// </summary>
    internal class InsertTextCommand : UndoableCommand
    {
        internal string insertedText;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tb">Underlaying textbox</param>
        /// <param name="insertedText">Text for inserting</param>
        public InsertTextCommand(TextSource ts, string insertedText)
            : base(ts)
        {
            this.insertedText = insertedText;
        }

        /// <summary>
        /// Undo operation
        /// </summary>
        public override void Undo()
        {
            ts.CurrentTB.Selection.Start = sel.Start;
            ts.CurrentTB.Selection.End = lastSel.Start;
            ts.OnTextChanging();
            ClearSelectedCommand.ClearSelected(ts);
            base.Undo();
        }

        /// <summary>
        /// Execute operation
        /// </summary>
        public override void Execute()
        {
            ts.OnTextChanging(ref insertedText);
            InsertText(insertedText, ts);
            base.Execute();
        }

        internal static void InsertText(string insertedText, TextSource ts)
        {
            var tb = ts.CurrentTB;
            try
            {
                tb.Selection.BeginUpdate();
                char cc = '\x0';
                if (ts.Count == 0)
                    InsertCharCommand.InsertLine(ts);
                tb.ExpandBlock(tb.Selection.Start.iLine);
                foreach (char c in insertedText)
                    InsertCharCommand.InsertChar(c, ref cc, ts);
                ts.NeedRecalc(new TextSource.TextChangedEventArgs(0, 1));
            }
            finally
            {
                tb.Selection.EndUpdate();
            }
        }

        public override UndoableCommand Clone()
        {
            return new InsertTextCommand(ts, insertedText);
        }
    }
}
