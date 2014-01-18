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

namespace YAMP
{
    /// <summary>
    /// Represents the current state of a context.
    /// </summary>
    public sealed class State
    {
        #region Members

        Runtime _runtime;

        #endregion

        #region Events

        /// <summary>
        /// Fires once a plugin has been changed (add, remove).
        /// </summary>
        public event EventHandler<ChangeEventArgs> PluginChanged;

        /// <summary>
        /// Fires once a constant has been changed (add, remove).
        /// </summary>
        public event EventHandler<ChangeEventArgs> ConstantChanged;

        /// <summary>
        /// Fires once a function has been changed (add, remove).
        /// </summary>
        public event EventHandler<ChangeEventArgs> FunctionChanged;

        /// <summary>
        /// Fires once a variable has been changed (add, remove, change).
        /// </summary>
        public event EventHandler<ChangeEventArgs> VariableChanged;

        /// <summary>
        /// Fires once a type has been changed (add, remove).
        /// </summary>
        public event EventHandler<ChangeEventArgs> TypeChanged;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a state assigned to a context.
        /// </summary>
        /// <param name="ctx">The parent context.</param>
        internal State(Runtime ctx)
        {
            _runtime = ctx;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the currently stored variables.
        /// </summary>
        public IEnumerable<KeyValuePair<String, Dynamic>> Variables
        {
            get { return _runtime.Core.Context.Variables; }
        }

        /// <summary>
        /// Gets the currently accessible functions.
        /// </summary>
        public IEnumerable<IFunction> Functions
        {
            get 
            {
                foreach(var f in _runtime.Core.Extensions.Functions)
                    yield return f;

                foreach (var f in _runtime.Core.Context.Functions)
                    yield return f;
            }
        }

        /// <summary>
        /// Gets the currently installed plugins.
        /// </summary>
        public IEnumerable<YPlugin> Plugins
        {
            get 
            {
                foreach (var plugin in _runtime.Core.Extensions)
                    yield return plugin;
            }
        }

        /// <summary>
        /// Gets the currently accessible constants.
        /// </summary>
        public IEnumerable<IConstant> Constants
        {
            get
            {
                foreach (var c in _runtime.Core.Extensions.Constants)
                    yield return c;

                foreach (var c in _runtime.Core.Context.Constants)
                    yield return c;
            }
        }

        #endregion

        #region Event Raisers

        internal void RaisePluginChanged(String name, ChangeState state)
        {
            if (PluginChanged != null)
                PluginChanged(this, new ChangeEventArgs { Modification = state, Name = name });

            Debug.WriteLine("PLUGIN ( " + name + " ) CHANGED ( " + state + " ) ...");
        }

        internal void RaiseConstantChanged(String name, ChangeState state)
        {
            if (ConstantChanged != null)
                ConstantChanged(this, new ChangeEventArgs { Modification = state, Name = name });

            Debug.WriteLine("CONSTANT ( " + name + " ) CHANGED ( " + state + " ) ...");
        }

        internal void RaiseFunctionChanged(String name, ChangeState state)
        {
            if (FunctionChanged != null)
                FunctionChanged(this, new ChangeEventArgs { Modification = state, Name = name });

            Debug.WriteLine("FUNCTION ( " + name + " ) CHANGED ( " + state + " ) ...");
        }

        internal void RaiseVariableChanged(String name, ChangeState state)
        {
            if (VariableChanged != null)
                VariableChanged(this, new ChangeEventArgs { Modification = state, Name = name });

            Debug.WriteLine("VARIABLE ( " + name + " ) CHANGED ( " + state + " ) ...");
        }

        internal void RaiseTypeChanged(String name, ChangeState state)
        {
            if (TypeChanged != null)
                TypeChanged(this, new ChangeEventArgs { Modification = state, Name = name });

            Debug.WriteLine("TYPE ( " + name + " ) CHANGED ( " + state + " ) ...");
        }

        #endregion

        #region Nested

        /// <summary>
        /// Represents all information about the change
        /// of a module.
        /// </summary>
        public class ChangeEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets the name of the module that changed.
            /// </summary>
            public String Name { get; set; }

            /// <summary>
            /// Gets or sets the modification on the module.
            /// </summary>
            public ChangeState Modification { get; set; }
        }

        #endregion
    }
}
