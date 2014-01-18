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
using YAMP.Core;
using YAMP.Parser;

namespace YAMP
{
    /// <summary>
    /// Represents a class to encapsulate all important settings.
    /// </summary>
    [Serializable]
    public sealed class Settings
    {
        #region Members

        static readonly Settings _default = new Settings();

        String _ans;
        Boolean _printLog;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new set of configurations.
        /// </summary>
        public Settings()
        {
            OptimizationLevel = Optimization.Full;
            DefaultVariableName = "ans";
            PrintLog = false;
            CatchExceptions = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the default configuration.
        /// </summary>
        public static Settings Default
        {
            get { return _default; }
        }

        /// <summary>
        /// Gets or sets if exceptions should be catched internally.
        /// The exceptions will then be set as results of a query.
        /// </summary>
        public Boolean CatchExceptions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if the default variable should be used to
        /// store the last unsaved result of a statement.
        /// </summary>
        public Boolean UseDefaultVariable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if log information (installing, uninstalling, ...)
        /// information should be printed in e.g. a REPL.
        /// </summary>
        public Boolean PrintLog
        {
            get { return _printLog; }
            set
            {
                if (value != _printLog)
                {
                    _printLog = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of default variable if nothing is
        /// assigned.
        /// </summary>
        public String DefaultVariableName
        {
            get { return _ans; }
            set 
            {
                if(Chars.IsIdentifier(value)) 
                    _ans = value; 
            }
        }

        /// <summary>
        /// Gets or sets the optimization level.
        /// </summary>
        public Optimization OptimizationLevel
        {
            get;
            set;
        }

        #endregion
    }
}
