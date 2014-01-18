using System;
using YAMP.Parser;
using YAMP.Core;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents a variable expression.
    /// </summary>
    sealed class VariableExpression : Expression
    {
        #region Members

        string _name;

        #endregion

        #region ctor

        public VariableExpression(Token name)
            : base(name)
        {
            _name = name.ValueAsString;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the statement should be assigned.
        /// </summary>
        public override bool IsAssigned
        {
            get { return false; }
        }

        #endregion

        #region Methods

        public override Dynamic Evaluate(RunContext ctx)
        {
            var value = ctx.GetVariable(_name);

            if (value != null)
                return value;

            var ctn = ctx.GetConstant(_name);

            if (ctn != null)
                return new Dynamic(ctn.Value);

            var f = ctx.GetFunction(_name);

            if (f != null)
                return new Dynamic(f);

            throw new YException("The variable " + _name + " could not be found!");
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override string ToCode()
        {
            return Token.ValueAsString;
        }

        #endregion
    }
}
