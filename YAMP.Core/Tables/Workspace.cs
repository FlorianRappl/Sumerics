using System;
using YAMP.Core;
using YAMP.Types;

namespace YAMP.Tables
{
    /// <summary>
    /// Represents the workspace, i.e. the working
    /// container for variables, functions, ...
    /// </summary>
    sealed class Workspace
    {
        #region Members

        readonly Runtime _runtime;
        readonly Libraries _libraries;
        readonly Context _context;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a workspace assigned to a context.
        /// </summary>
        /// <param name="ctx">The parent context.</param>
        internal Workspace(Runtime ctx)
        {
            _runtime = ctx;

            _libraries = new Libraries();
            _libraries.Notify = ctx.Objects.RaisePluginChanged;
            _libraries.Constants.Notify = ctx.Objects.RaiseConstantChanged;
            _libraries.Functions.Notify = ctx.Objects.RaiseFunctionChanged;

            _context = new Context();
            _context.Constants.Notify = ctx.Objects.RaiseConstantChanged;
            _context.Functions.Notify = ctx.Objects.RaiseFunctionChanged;
            _context.Variables.Notify = ctx.Objects.RaiseFunctionChanged;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the global variables.
        /// </summary>
        public Context Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Gets the assigned context.
        /// </summary>
        public Runtime Runtime
        {
            get { return _runtime; }
        }

        /// <summary>
        /// Gets the installed extensions.
        /// </summary>
        public Libraries Extensions
        {
            get { return _libraries; }
        }

        #endregion
    }
}
