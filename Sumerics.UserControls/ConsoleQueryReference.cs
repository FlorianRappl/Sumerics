namespace Sumerics.Controls
{
    using FastColoredTextBoxNS;
    using System;
    using System.Drawing;
    using System.Text;

    public class ConsoleQueryReference
    {
        readonly String _originalQuery;
        readonly OutputRegion _outputRegion;
        readonly FastColoredTextBox _control;

        internal ConsoleQueryReference(String query, OutputRegion outputRegion, FastColoredTextBox control)
        {
            _originalQuery = query;
            _outputRegion = outputRegion;
            _control = control;
        }

        public String OriginalQuery
        {
            get { return _originalQuery; }
        }

        Int32 CurrentChars
        {
            get { return _control.Width / _control.CharWidth; }
        }

        public void SetRunning()
        {
            _outputRegion.Style.ForeBrush = new SolidBrush(Color.SteelBlue);
            _outputRegion.Text = "Evaluating ...";
        }

        public void SetError(String message)
        {
            _outputRegion.Style.ForeBrush = new SolidBrush(Color.PaleVioletRed);
            _outputRegion.Text = message;
        }

        public void SetVoid()
        {
            _outputRegion.Text = String.Empty;
        }

        public void SetText(String result)
        {
            var lc = result.Split('\n').Length;
            _outputRegion.Style.ForeBrush = new SolidBrush(Color.DarkGray);
            _outputRegion.Fold = lc > 2;
            _outputRegion.Text = String.Format("[ {0} Lines of text ]\n", lc) + result;
        }

        public void SetMatrix(String[,] values)
        {
            var length = ComputeLargestStringContent(values);
            var cols = values.GetLength(0);
            var rows = values.GetLength(1);
            var requireFormat = TooManyColumns(cols, length);
            var mtxt = requireFormat ? GetFormattedText(values, length) : GetPlainText(values, length);
            _outputRegion.Style.ForeBrush = new SolidBrush(Color.DarkGray);
            _outputRegion.Fold = rows > 1 || requireFormat;
            _outputRegion.Text = String.Format("[ {0} x {1} Matrix ]", rows, cols) + mtxt;
        }

        public void SetResult(String value)
        {
            _outputRegion.Style.ForeBrush = new SolidBrush(Color.DarkGray);
            _outputRegion.Text = value;
        }

        Boolean TooManyColumns(Int32 cols, Int32 length)
        {
            var colLength = length + 3;
            return cols * colLength > CurrentChars;
        }

        String GetFormattedText(String[,] m, Int32 length)
        {
            var sb = new StringBuilder();
            var colLength = length + 1;
            var cols = m.GetLength(0);
            var rows = m.GetLength(1);
            var w = Math.Max(1, CurrentChars / colLength - 1);
            var loops = cols / w;

            if (cols > loops * w)
            {
                loops++;
            }

            for (var k = 0; k < loops; k++)
            {
                var c = k * w;
                var max = c + Math.Min(w - 1, cols - c);
                sb.AppendLine();
                sb.AppendLine();

                if (c != max)
                {
                    sb.AppendFormat("Columns {0} through {1}", c, max);
                }
                else
                {
                    sb.Append("Column ").Append(c);
                }

                sb.AppendLine();

                for (var i = 0; i < rows; i++)
                {
                    sb.AppendLine();

                    for (var j = c; j < max; j++)
                    {
                        sb.Append(m[j, i].PadLeft(colLength));
                    }
                }
            }

            return sb.ToString();
        }

        String GetPlainText(String[,] m, Int32 length)
        {
            var sb = new StringBuilder();
            var colLength = length + 1;
            var cols = m.GetLength(0);
            var rows = m.GetLength(1);

            for (var i = 0; i < rows; i++)
            {
                sb.AppendLine();

                for (var j = 0; j < cols; j++)
                {
                    sb.Append(m[j, i].PadLeft(colLength));
                }
            }

            return sb.ToString();
        }

        static Int32 ComputeLargestStringContent(String[,] values)
        {
            var cols = values.GetLength(0);
            var rows = values.GetLength(1);
            var length = 0;

            for (var j = 0; j != rows; j++)
            {
                var sum = 0;

                for (var i = 0; i != cols; i++)
                {
                    sum += values[i, j].Length;
                }

                length = Math.Max(sum, length);
            }

            return length;
        }
    }
}
