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

namespace YAMP.Core
{
    /// <summary>
    /// Represents a sample READ-EVALUATE-PRINT-LOOP
    /// for YAMP².
    /// </summary>
    public sealed class Repl
    {
        #region Members

        Runtime _context;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new REPL instance for the given
        /// context.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repl(Runtime context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new REPL instance with a new context.
        /// The context is created with default options.
        /// </summary>
        public Repl()
        {
            _context = new Runtime();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the assigned YContext instance.
        /// </summary>
        public Runtime Context
        {
            get { return _context; }
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Runs the REPL synchronously.
        /// </summary>
        /// <param name="console">The console to run in.</param>
        public void Run(IConsole console)
        {
            console.Startup();

            while (!console.CancelRequested)
            {
                console.Write(">>> ");
                var cmd = console.ReadCommand();

                if (cmd == null || cmd == "exit")
                    break;

                var v = _context.RunQuery(cmd);

                if (v.CompilationInfo.HasErrors)
                {
                    console.FontColor = Color.Magenta;
                    console.WriteLine(v.ToString());
                }
                else if (v.Failed)
                {
                    console.FontColor = Color.Red;
                    console.WriteLine(v.ToString());
                }
                else if (v.Result.Value != Nothing.ToReturn)
                {
                    console.FontColor = Color.Green;
                    console.WriteLine(v.ToString());
                }

                console.ResetColor();
            }

            console.Exit();
        }

        #endregion 
    }
}
