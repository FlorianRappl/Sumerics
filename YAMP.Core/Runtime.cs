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
using System.Collections.Generic;
using System.Diagnostics;
using YAMP.Core;
using YAMP.Parser;
using YAMP.Tables;

namespace YAMP
{
    /// <summary>
    /// Represents a context to run in.
    /// </summary>
    public sealed class Runtime
    {
        #region Members

        State _state;
        Settings _settings;
        Workspace _core;

        static Runtime _dummy;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new YAMP.Core runtime.
        /// </summary>
        /// <param name="enableMatrix">Sets if matrices should be enabled.</param>
        /// <param name="enableFunctions">Sets if additional functions should be loaded.</param>
        /// <param name="enableConstants">Sets if additional constants should be loaded.</param>
        /// <param name="stdout">Sets a stdout redirection action.</param>
        /// <param name="stdin">Sets a stdin demand function.</param>
        /// <param name="useDefaultVariable">Sets if non-stored computations should be saved.</param>
        public Runtime(Boolean enableMatrix = true,
                        Boolean enableFunctions = true,
                        Boolean enableConstants = true,
                        Boolean useDefaultVariable = false)
        {
            _state = new State(this);
            _core = new Workspace(this);
            _settings = Settings.Default;
            _settings.UseDefaultVariable = useDefaultVariable;

            Install(new StandardLibrary(
                withMatrix:         enableMatrix,
                withConstants:      enableConstants,
                withTrigonometric:  enableFunctions,
                withRandom:         enableFunctions,
                withMathematics:    enableFunctions,
                withConversion:     enableFunctions,
                withLogic:          enableFunctions));
        }

        #endregion

        #region Internal properties

        /// <summary>
        /// Gets a fresh runcontext to be used.
        /// </summary>
        [DebuggerHidden]
        internal RunContext Context
        {
            get { return new RunContext(_core); }
        }

        /// <summary>
        /// Gets the workspace core.
        /// </summary>
        internal Workspace Core
        {
            get { return _core; }
        }

        /// <summary>
        /// Gets the dummy context.
        /// </summary>
        internal static Runtime Dummy
        {
            get { return _dummy ?? new Runtime(); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the configuration to use.
        /// </summary>
        public Settings Configuration
        {
            get { return _settings; }
            set { _settings = value; }
        }

        /// <summary>
        /// Gets the objects state (workspace and more) for this context.
        /// </summary>
        public State Objects
        {
            get { return _state; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the whole context, i.e. uninstalls all plugins,
        /// functions and everything that is not related to the core.
        /// </summary>
        /// <returns>The current context.</returns>
        public Runtime Clear()
        {
            _core.Context.Functions.Clear();
            _core.Context.Constants.Clear();

            if (_core.Extensions.Count > 1)
            {
                var plugins = new List<YPlugin>();

                foreach (var plugin in _core.Extensions)
                    if (!(plugin is StandardLibrary))
                        plugins.Add(plugin);

                for (int i = 0; i < plugins.Count; i++)
                    _core.Extensions.Remove(plugins[i]);
            }

            _core.Context.Variables.Clear();
            return this;
        }

        /// <summary>
        /// Installs the given plugin.
        /// </summary>
        /// <param name="plugin">The plugin to install.</param>
        /// <returns>The current context.</returns>
        public Runtime Install(YPlugin plugin)
        {
            Trace.Write("Installing plugin " + plugin.Name + " ... ");
            var ticks = Environment.TickCount;
            _core.Extensions.Add(plugin);
            ticks = Environment.TickCount - ticks;
            Trace.WriteLine("done (" + ticks + " ms)");
            return this;
        }

        /// <summary>
        /// Uninstalls the given plugin.
        /// </summary>
        /// <param name="plugin">The plugin to uninstall.</param>
        /// <returns>The current context.</returns>
        public Runtime Uninstall(YPlugin plugin)
        {
            Trace.Write("Uninstalling plugin " + plugin.Name + " ... ");
            var ticks = Environment.TickCount;
            _core.Extensions.Remove(plugin);
            ticks = Environment.TickCount - ticks;
            Trace.WriteLine("done (" + ticks + " ms)");
            return this;
        }

        /// <summary>
        /// Adds the given function.
        /// </summary>
        /// <param name="function">The function to register.</param>
        /// <returns>The current context.</returns>
        public Runtime Add(IFunction function)
        {
            _core.Context.Functions.Add(function);
            return this;
        }

        /// <summary>
        /// Removes the given function.
        /// </summary>
        /// <param name="function">The function to unregister.</param>
        /// <returns>The current context.</returns>
        public Runtime Remove(IFunction function)
        {
            _core.Context.Variables.RemoveFrom(function);
            _core.Context.Functions.Remove(function);
            return this;
        }

        /// <summary>
        /// Adds the given function.
        /// </summary>
        /// <param name="constant">The function to register.</param>
        /// <returns>The current context.</returns>
        public Runtime Add(IConstant constant)
        {
            _core.Context.Constants.Add(constant);
            return this;
        }

        /// <summary>
        /// Removes the given function.
        /// </summary>
        /// <param name="constant">The function to unregister.</param>
        /// <returns>The current context.</returns>
        public Runtime Remove(IConstant constant)
        {
            _core.Context.Constants.Remove(constant);
            return this;
        }

        /// <summary>
        /// Runs the query with the given source code.
        /// </summary>
        /// <param name="source">The code to interpret.</param>
        /// <returns>The query.</returns>
        public MathQuery RunQuery(String source)
        {
            var q = new MathQuery(this);
            q.Compile(source);

            if (q.CanRun)
                q.Run();

            return q;
        }

        /// <summary>
        /// Runs the query asynchronously with the given source code.
        /// </summary>
        /// <param name="source">The code to interpret.</param>
        /// <returns>The query.</returns>
        public MathQuery RunQueryAsync(String source)
        {
            var q = new MathQuery(this);
            q.Compile(source);

            if (q.CanRun)
                q.RunAsync();

            return q;
        }

        #endregion

        #region Quick Evaluation

        /// <summary>
        /// Runs a quick evaluation over the given query in a
        /// dummy context.
        /// </summary>
        /// <param name="query">The query to evaluate.</param>
        /// <returns>The result of the evaluation.</returns>
        public static Object Evaluate(String query)
        {
            if (_dummy == null)
                _dummy = new Runtime(enableMatrix: false);
            else
                _dummy.Clear();

            var q = new MathQuery(_dummy);
            q.CompileAndRun(query);
            Debug.WriteLine(q.CompilationInfo);
            return q.Result.Value;
        }

        #endregion
    }
}
