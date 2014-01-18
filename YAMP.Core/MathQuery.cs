/*
	Copyright (c) 2012-2013, Florian Rappl et al.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
		  may be used to endorse or promote products derived from this
		  software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using YAMP.Parser;
using YAMP.Report;
using YAMP.Statements;

namespace YAMP
{
    /// <summary>
    /// Represents a query to be evaluated.
    /// </summary>
    public sealed class MathQuery
    {
        #region Members

        Runtime _ctx;
        String _query;
        Boolean _running;
        Dynamic _result;
        StatementList _statements;
        ParserReport _report;
        Task<Dynamic> _async;
        CancellationTokenSource _cts;

        #endregion

        #region Events

        /// <summary>
        /// Fires when the query is completed.
        /// </summary>
        public event EventHandler Completed;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new query.
        /// </summary>
        /// <param name="ctx">The context of the query.</param>
        internal MathQuery(Runtime ctx)
        {
            _ctx = ctx;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the status of the query - has it failed?
        /// </summary>
        public Boolean Failed
        {
            get { return _result != null && _result.Value is Exception; }
        }

        /// <summary>
        /// Gets the status of the query - could it be evaluated now?
        /// </summary>
        public Boolean CanRun 
        {
            get { return _running == false && Statements != null && CompilationInfo != null && CompilationInfo.HasErrors == false; }
        }

        /// <summary>
        /// Gets a report of the compilation.
        /// </summary>
        public ParserReport CompilationInfo
        {
            get { return _report; }
        }

        /// <summary>
        /// Gets the result of the query.
        /// </summary>
        public Dynamic Result
        {
            get { return _result ?? new Dynamic(); }
        }

        /// <summary>
        /// Gets the text of the query to evaluate.
        /// </summary>
        public String Text
        {
            get { return _query; }
        }

        /// <summary>
        /// Gets or sets the scope of the query.
        /// </summary>
        internal StatementList Statements 
        {
            get { return _statements; }
        }

        #endregion

        #region Static methods

        /// <summary>
        /// A quick determination if the given query compiles.
        /// </summary>
        /// <param name="query">The query to check.</param>
        /// <returns>True if the query would compile without errors, otherwise false.</returns>
        public static Boolean IsCompiling(String query)
        {
            return Frontend.Run(query).ErrorCount == 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compiles the query synchronous.
        /// </summary>
        /// <param name="query">The query to evaluate.</param>
        [DebuggerStepThrough]
        internal void Compile(String query)
        {
            if (_report == null)
            {
                _query = query;
                var cmp = new Frontend(query);
                cmp.Run();
                _statements = cmp.Result;
                _report = new ParserReport(cmp);

                if (cmp.HasErrors == false)
                {
                    var opt = new Backend(this);
                    opt.Optimize(_ctx.Configuration.OptimizationLevel);
                }
            }
        }

        /// <summary>
        /// Compiles and runs the query synchronous.
        /// </summary>
        /// <param name="query">The query to evaluate.</param>
        [DebuggerStepThrough]
        internal void CompileAndRun(String query)
        {
            Compile(query);
            Run();
        }

        /// <summary>
        /// Compiles and runs the query asynchronous.
        /// </summary>
        /// <param name="query">The query to evaluate.</param>
        [DebuggerStepThrough]
        internal void CompileAndRunAsync(String query)
        {
            Compile(query);
            RunAsync();
        }

        /// <summary>
        /// Runs the query synchronous.
        /// </summary>
        public void Run()
        {
            if (CompilationInfo.HasErrors)
                _result = new Dynamic(new Exception("Some compilation errors occured."));
            else if (!_running)
            {
                try
                {
                    _running = true;
                    _result = Statements.Evaluate(_ctx.Context);
                }
                catch (Exception ex)
                {
                    _result = new Dynamic(ex);
                }
                finally
                {
                    _running = false;
                }
            }
        }

        /// <summary>
        /// Runs the query asynchronous.
        /// </summary>
        public void RunAsync()
        {
            if (CompilationInfo.HasErrors)
                _result = new Dynamic(new Exception("Some compilation errors occured."));
            else if (!_running)
            {
                _running = true;
                _cts = new CancellationTokenSource();
                _async = new Task<Dynamic>(() => Statements.Evaluate(_ctx.Context), _cts.Token);
                _async.ContinueWith(result =>
                {
                    if (result.IsFaulted)
                        _result = new Dynamic(result.Exception.InnerException);
                    else
                        _result = result.Result;

                    _running = false;

                    if (Completed != null)
                        Completed(this, EventArgs.Empty);
                }, TaskScheduler.Current);
                _async.Start();
            }
        }

        /// <summary>
        /// Cancels the currently running query.
        /// </summary>
        public void Cancel()
        {
            if (_running)
            {
                if (_cts != null)
                    _cts.Cancel();
            }
        }

        /// <summary>
        /// Stringifies the result as a string.
        /// </summary>
        /// <returns>The resulting string.</returns>
        public override String ToString()
        {
            if (_report.HasErrors)
                return _report.ToString();

            return _result.Codify();
        }

        #endregion
    }
}
