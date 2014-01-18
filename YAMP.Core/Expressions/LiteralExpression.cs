using System;
using YAMP.Parser;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents a literal expression.
    /// </summary>
    sealed class LiteralExpression : Expression
    {
        #region ctor

        public LiteralExpression(Token token)
            : base(token)
        {
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override string ToCode()
        {
            return Token.ToCode();
        }

        #endregion
    }
}
