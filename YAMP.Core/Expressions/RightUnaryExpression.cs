using System;
using YAMP.Parser;
using YAMP.Core;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents a right unary expression.
    /// </summary>
    abstract class RightUnaryExpression : Expression
    {
        #region Members

        Expression _expression;

        #endregion

        #region ctor

        public RightUnaryExpression(Token token, Expression expression)
            : base(token)
        {
            _expression = expression ?? EmptyExpression.Instance;
        }

        #endregion

        #region Creator

        /// <summary>
        /// Creates a new right unary expression.
        /// </summary>
        /// <param name="token">The token of the unary expression.</param>
        /// <param name="left">The expression.</param>
        /// <returns>The unary expression.</returns>
        public static RightUnaryExpression Create(Token token, Expression exp)
        {
            switch (token.Type)
            {
                case TokenType.Increment:
                    return new IncExpression(token, exp);
                case TokenType.Decrement:
                    return new DecExpression(token, exp);
                case TokenType.Factorial:
                    return new FacExpression(token, exp);
                case TokenType.Transpose:
                    return new TrnExpression(token, exp);
                case TokenType.Adjungate:
                    return new AdjExpression(token, exp);
            }

            throw new Exception("Obviously a problem with the parser occured. The token " + token + " should not have landed here. Please report this error!");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the used expression.
        /// </summary>
        public Expression Expression
        {
            get { return _expression; }
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override string ToCode()
        {
            return string.Format("({1}{0})", Token.ToCode(), _expression.ToCode());
        }

        #endregion

        #region Nested expressions

        sealed class IncExpression : RightUnaryExpression
        {
            BinaryExpression assignment;

            public IncExpression(Token token, Expression exp)
                : base(token, exp)
            {
                assignment = BinaryExpression.Create(Token.Assignment(TokenType.Add, token.Row, token.Column), 
                    exp, new LiteralExpression(Token.Number(1L, 0, 0)));
            }

            /// <summary>
            /// Gets if the statement should be assigned.
            /// </summary>
            public override bool IsAssigned
            {
                get { return false; }
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var x = _expression.Evaluate(ctx);
                assignment.Evaluate(ctx);
                return x;
            }
        }

        sealed class DecExpression : RightUnaryExpression
        {
            BinaryExpression assignment;

            public DecExpression(Token token, Expression exp)
                : base(token, exp)
            {
                assignment = BinaryExpression.Create(Token.Assignment(TokenType.Subtract, token.Row, token.Column), 
                    exp, new LiteralExpression(Token.Number(1L, 0, 0)));
            }

            /// <summary>
            /// Gets if the statement should be assigned.
            /// </summary>
            public override bool IsAssigned
            {
                get { return false; }
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var x = _expression.Evaluate(ctx);
                assignment.Evaluate(ctx);
                return x;
            }
        }

        sealed class FacExpression : RightUnaryExpression
        {
            public FacExpression(Token token, Expression exp)
                : base(token, exp)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var x = _expression.Evaluate(ctx);
                return ~x;
            }
        }

        sealed class TrnExpression : RightUnaryExpression
        {
            public TrnExpression(Token token, Expression exp)
                : base(token, exp)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var x = _expression.Evaluate(ctx);
                return x.Transpose();
            }
        }

        sealed class AdjExpression : RightUnaryExpression
        {
            public AdjExpression(Token token, Expression exp)
                : base(token, exp)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var x = _expression.Evaluate(ctx);
                return x.Adjungate();
            }
        }

        #endregion
    }
}
