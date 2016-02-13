namespace Sumerics
{
    using FastColoredTextBoxNS;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using System.Threading;
    using YAMP;

    public class QueryResultViewModel
    {
        #region Fields

        Value _result;
        Exception _exception;
		Boolean _running;

		#endregion

		#region ctor

		public QueryResultViewModel(String query, OutputRegion region)
		{
			Region = region;
			Query = query;
			Running = true;
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
			get;
			private set;
		}

		public OutputRegion Region
		{
			get;
			private set;
		}

        public Value Value
        {
            get { return _result; }
            set
            {
                _result = value;
                Running = false;
                RaisePropertyChanged();
            }
        }

        public Exception Error
        {
            get { return _exception; }
            set
            {
                _exception = value;
                Running = false;
                RaisePropertyChanged();
            }
        }

        public Boolean Running
        {
            get { return _running; }
            set
            {
                _running = value;

                if (_running)
                {
                    if (!runningQueries.Contains(this))
                    {
                        AddRunningQuery(this);
                    }
                }
                else
                {
                    if (runningQueries.Contains(this))
                    {
                        RemoveRunningQuery(this);
                    }
                }

                RaisePropertyChanged();
            }
		}

        #endregion

        #region Static Class

        static List<QueryResultViewModel> runningQueries = new List<QueryResultViewModel>();

        public static event EventHandler RunningQueriesChanged;

        public static Boolean HasRunningQueries
        {
            get { return runningQueries.Count > 0; }
        }

        public static IEnumerable<QueryResultViewModel> RunningQueries
        {
            get
            {
                foreach (var query in runningQueries)
                {
                    yield return query;
                }
            }
        }

        static void AddRunningQuery(QueryResultViewModel qrvm)
        {
            runningQueries.Add(qrvm);

            if (runningQueries.Count == 1 && RunningQueriesChanged != null)
                RunningQueriesChanged(qrvm, EventArgs.Empty);
        }

        static void RemoveRunningQuery(QueryResultViewModel qrvm)
        {
            runningQueries.Remove(qrvm);

            if (runningQueries.Count == 0 && RunningQueriesChanged != null)
                RunningQueriesChanged(qrvm, EventArgs.Empty);
        }

        #endregion

		#region Methods

        public void Cancel()
        {
            if (Running && Thread != null)
                Thread.Abort();
        }

		protected void RaisePropertyChanged()
        {
			if (Running)
			{
				Region.Style.ForeBrush = new SolidBrush(Color.SteelBlue);
				Region.Text = "Evaluating ...";
			}
			else
			{
				if (Value != null)
				{
					Region.Style.ForeBrush = new SolidBrush(Color.DarkGray);

                    if (Value is MatrixValue)
					{
                        var m = (MatrixValue)Value;
                        var length = m.ComputeLargestStringContent(Context);
                        var requireFormat = TooManyColumns(m.Columns, length);
                        Region.Fold = m.DimensionY > 1 || requireFormat;
                        var mtxt = requireFormat ? GetFormattedText(m, length) : GetPlainText(m, length);
                        Region.Text = string.Format("[ {0} x {1} Matrix ]", m.DimensionY, m.DimensionX) + mtxt;
                    }
					else if (Value is StringValue)
					{
						var s = (StringValue)Value;
						var lc = s.Value.Split('\n').Length;
						Region.Fold = lc > 2;
                        Region.Text = string.Format("[ {0} Lines of text ]\n", lc) + s.Value;
					}
					else
					{
						Region.Text = Value.ToString(Context);
					}
				}
				else if (Error != null)
				{
					Region.Style.ForeBrush = new SolidBrush(Color.PaleVioletRed);
					Region.Text = Error.Message;
				}
				else
				{
					Region.Text = string.Empty;
				}
			}
        }

        #endregion

        #region Formatters

        bool TooManyColumns(int cols, int length)
        {
            var w = Region.CurrentChars;
            var colLength = length + 3;
            return cols * colLength > w;
        }

		string GetFormattedText(MatrixValue m, int length)
		{
            var sb = new StringBuilder();
            var colLength = length + 1;

            var w = Region.CurrentChars;
			var cols = Math.Max(1, w / colLength - 1);
			var loops = m.DimensionX / cols;

			if (m.DimensionX > loops * cols)
				loops++;

			for (var k = 0; k < loops; k++)
			{
				var c = k * cols + 1;
				var max = c + Math.Min(cols - 1, m.DimensionX - c);
				sb.AppendLine();
				sb.AppendLine();
                    
                if (c != max)
                    sb.AppendFormat("Columns {0} through {1}", c, max);
                else
                    sb.Append("Column ").Append(c);

				sb.AppendLine();

				for (var i = 1; i <= m.DimensionY; i++)
				{
					sb.AppendLine();

					for (var j = c; j <= max; j++)
						sb.Append(m[i, j].ToString(Context).PadLeft(colLength));
				}
			}

			return sb.ToString();
		}

        string GetPlainText(MatrixValue m, int length)
        {
            var sb = new StringBuilder();
            var colLength = length + 1;

            for (var i = 1; i <= m.DimensionY; i++)
            {
                sb.AppendLine();

                for (var j = 1; j <= m.DimensionX; j++)
                    sb.Append(m[i, j].ToString(Context).PadLeft(colLength));
            }

            return sb.ToString();
        }

		#endregion
    }
}
