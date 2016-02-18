namespace Sumerics.Controls
{
    using FastColoredTextBoxNS;
    using System;

    /// <summary>
    /// Autocomplete item for code snippets
    /// </summary>
    /// <remarks>Snippet can contain special char ^ for caret position.</remarks>
    public class SnippetAutocompleteItem : AutocompleteItem
    {
        public SnippetAutocompleteItem(String snippet)
        {
            Text = snippet.Replace("\r", "");
            ToolTip = "Code snippet: " + Text;
        }

        public override String ToString()
        {
            return Text.Replace("\n", " ").Replace("^", "");
        }

        public override String GetTextForReplace()
        {
            return Text;
        }

        public override void OnSelected(Object sender, SelectedEventArgs e)
        {
            var popupMenu = (AutocompletePopup)sender;
            e.Tb.BeginUpdate();
            e.Tb.Selection.BeginUpdate();
            var p1 = popupMenu.Fragment.Start;
            var p2 = e.Tb.Selection.Start;

            if (e.Tb.AutoIndent)
            {
                for (var iLine = p1.iLine + 1; iLine <= p2.iLine; iLine++)
                {
                    e.Tb.Selection.Start = new Place(0, iLine);
                    e.Tb.DoAutoIndent(iLine);
                }
            }

            e.Tb.Selection.Start = p1;

            //move caret position right and find char ^
            while (e.Tb.Selection.CharBeforeStart != '^')
            {
                if (!e.Tb.Selection.GoRightThroughFolded())
                {
                    break;
                }
            }

            e.Tb.Selection.EndUpdate();
            e.Tb.EndUpdate();
        }

        public override CompareResult Compare(String fragmentText)
        {
            if (Text.StartsWith(fragmentText, StringComparison.InvariantCultureIgnoreCase) && Text != fragmentText)
            {
                return CompareResult.Visible;
            }

            return CompareResult.Hidden;
        }
    }
}
