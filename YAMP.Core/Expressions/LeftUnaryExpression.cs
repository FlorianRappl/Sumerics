using System;
using YAMP.Parser;
using YAMP.Core;

namespace YAMP.Expressions
{
    abstract class LeftUnaryExpression : Expression
    {
        #region Members

        Expression _expression;

        #endregion

        #region ctor

        public LeftUnaryExpression(Token token, Expression expression)
            : base(token)
        {
            _expression = expression ?? EmptyExpression.Instance;
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

        #region Creator

        /// <summary>
        /// Creates a new left unary expression.
        /// </summary>
        /// <param name="token">The token of the unary expression.</param>
        /// <param name="left">The expression.</param>
        /// <returns>The unary expression.</returns>
        public static LeftUnaryExpression Create(Token token, Expression exp)
        {
            switch (token.Type)
            {
                case TokenType.Increment:
                    return new IncExpression(token, exp);
                case TokenType.Decrement:
                    return new DecExpression(token, exp);
                case TokenType.Subtract:
                    return new NegExpression(token, exp);
                //case TokenType.Not:
                case TokenType.Factorial:
                    return new NotExpression(token, exp);
            }

            throw new Exception("Obviously a problem with the parser occured. The token " + token + " should not have landed here. Please report this error!");
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override string ToCode()
        {
            return string.Format("({0}{1})", Token.ToCode(), _expression.ToCode());
        }

        #endregion

        #region Nested expressions

        sealed class IncExpression : LeftUnaryExpression
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
                return assignment.Evaluate(ctx);
            }
        }

        sealed class DecExpression : LeftUnaryExpression
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
                return assignment.Evaluate(ctx);
            }
        }

        sealed class NegExpression : LeftUnaryExpression
        {
            public NegExpression(Token token, Expression exp)
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
                return -x;
            }
        }

        sealed class NotExpression : LeftUnaryExpression
        {
            public NotExpression(Token token, Expression exp)
                : base(token, exp)
            {
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
                return !x;
            }
        }

        #endregion
    }
}
