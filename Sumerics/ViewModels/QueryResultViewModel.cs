namespace Sumerics.ViewModels
{
    using Sumerics.Controls;
    using System;
    using System.Threading;
    using YAMP;

    public sealed class QueryResultViewModel : BaseViewModel
    {
        #region Fields

        readonly ConsoleQueryReference _cqr;
        readonly ParseContext _context;
        Value _result;
        Exception _exception;
		Boolean _running;

		#endregion

		#region ctor

        public QueryResultViewModel(ConsoleQueryReference cqr, ParseContext context)
		{
            _context = context;
			_cqr = cqr;
		}

		#endregion

        #region Properties

        public ParseContext Context 
        {
            get { return _context; }
        }

        public Thread Thread
        {
            get;
            set;
        }

		public String Query
		{
            get { return _cqr.OriginalQuery; }
		}

        public Value Value
        {
            get { return _result; }
            set 
            {
                _result = value; 
                
                if (value == null)
                {
                    App.Current.Dispatcher.Invoke(() => _cqr.SetVoid());
                }
                else if (value is MatrixValue)
                {
                    var matrix = (MatrixValue)value;
                    var values = new String[matrix.Columns, matrix.Rows];

                    for (int i = 0; i < matrix.Columns; i++)
                    {
                        for (int j = 0; j < matrix.Rows; j++)
                        {
                            values[i, j] = matrix[j + 1, i + 1].ToString(Context);
                        }
                    }

                    App.Current.Dispatcher.Invoke(() => _cqr.SetMatrix(values));
                }
                else if (value is StringValue)
                {
                    App.Current.Dispatcher.Invoke(() => _cqr.SetText(value.ToString(Context)));
                }
                else
                {
                    App.Current.Dispatcher.Invoke(() => _cqr.SetResult(value.ToString(Context)));
                }

                RaisePropertyChanged(); 
            }
        }

        public Exception Error
        {
            get { return _exception; }
            set 
            {
                _exception = value;

                if (_exception != null)
                {
                    App.Current.Dispatcher.Invoke(() => _cqr.SetError(_exception.Message));
                }
                
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
                    App.Current.Dispatcher.Invoke(() => _cqr.SetRunning());
                }

                RaisePropertyChanged(); 
            }
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
    }
}
