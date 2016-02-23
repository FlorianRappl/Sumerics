namespace Sumerics.ViewModels
{
    using System;
    using System.Threading;
    using YAMP;

    public sealed class QueryResultViewModel : BaseViewModel
    {
        #region Fields

        readonly String _query;
        Value _result;
        Exception _exception;
		Boolean _running;

		#endregion

		#region ctor

		public QueryResultViewModel(String query)
		{
			_query = query;
		}

		#endregion

        #region Properties

        public ParseContext Context 
        {
            get;
            set; 
        }

        public Thread Thread
        {
            get;
            set;
        }

		public String Query
		{
            get { return _query; }
		}

        public Value Value
        {
            get { return _result; }
            set { _result = value; RaisePropertyChanged(); }
        }

        public Exception Error
        {
            get { return _exception; }
            set { _exception = value; RaisePropertyChanged(); }
        }

        public Boolean Running
        {
            get { return _running; }
            set { _running = value; RaisePropertyChanged(); }
		}

        #endregion

		#region Methods

        public void Cancel()
        {
            if (Running && Thread != null)
            {
                Thread.Abort();
            }
        }

        #endregion

        #region Formatters

        //protected void RaisePropertyChanged()
        //{
        //    if (Running)
        //    {
        //        Region.Style.ForeBrush = new SolidBrush(Color.SteelBlue);
        //        Region.Text = "Evaluating ...";
        //    }
        //    else
        //    {
        //        if (Value != null)
        //        {
        //            Region.Style.ForeBrush = new SolidBrush(Color.DarkGray);

        //            if (Value is MatrixValue)
        //            {
        //                var m = (MatrixValue)Value;
        //                var length = m.ComputeLargestStringContent(Context);
        //                var requireFormat = TooManyColumns(m.Columns, length);
        //                Region.Fold = m.DimensionY > 1 || requireFormat;
        //                var mtxt = requireFormat ? GetFormattedText(m, length) : GetPlainText(m, length);
        //                Region.Text = String.Format("[ {0} x {1} Matrix ]", m.DimensionY, m.DimensionX) + mtxt;
        //            }
        //            else if (Value is StringValue)
        //            {
        //                var s = (StringValue)Value;
        //                var lc = s.Value.Split('\n').Length;
        //                Region.Fold = lc > 2;
        //                Region.Text = String.Format("[ {0} Lines of text ]\n", lc) + s.Value;
        //            }
        //            else
        //            {
        //                Region.Text = Value.ToString(Context);
        //            }
        //        }
        //        else if (Error != null)
        //        {
        //            Region.Style.ForeBrush = new SolidBrush(Color.PaleVioletRed);
        //            Region.Text = Error.Message;
        //        }
        //        else
        //        {
        //            Region.Text = String.Empty;
        //        }
        //    }
        //}

        //Boolean TooManyColumns(Int32 cols, Int32 length)
        //{
        //    var w = Region.CurrentChars;
        //    var colLength = length + 3;
        //    return cols * colLength > w;
        //}

        //String GetFormattedText(MatrixValue m, Int32 length)
        //{
        //    var sb = new StringBuilder();
        //    var colLength = length + 1;

        //    var w = Region.CurrentChars;
        //    var cols = Math.Max(1, w / colLength - 1);
        //    var loops = m.DimensionX / cols;

        //    if (m.DimensionX > loops * cols)
        //    {
        //        loops++;
        //    }

        //    for (var k = 0; k < loops; k++)
        //    {
        //        var c = k * cols + 1;
        //        var max = c + Math.Min(cols - 1, m.DimensionX - c);
        //        sb.AppendLine();
        //        sb.AppendLine();

        //        if (c != max)
        //        {
        //            sb.AppendFormat("Columns {0} through {1}", c, max);
        //        }
        //        else
        //        {
        //            sb.Append("Column ").Append(c);
        //        }

        //        sb.AppendLine();

        //        for (var i = 1; i <= m.DimensionY; i++)
        //        {
        //            sb.AppendLine();

        //            for (var j = c; j <= max; j++)
        //            {
        //                sb.Append(m[i, j].ToString(Context).PadLeft(colLength));
        //            }
        //        }
        //    }

        //    return sb.ToString();
        //}

        //String GetPlainText(MatrixValue m, Int32 length)
        //{
        //    var sb = new StringBuilder();
        //    var colLength = length + 1;

        //    for (var i = 1; i <= m.DimensionY; i++)
        //    {
        //        sb.AppendLine();

        //        for (var j = 1; j <= m.DimensionX; j++)
        //        {
        //            sb.Append(m[i, j].ToString(Context).PadLeft(colLength));
        //        }
        //    }

        //    return sb.ToString();
        //}

		#endregion
    }
}
