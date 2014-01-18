using System;
using YAMP.Parser;
using YAMP.Core;
using YAMP.Attributes;
using System.Collections.Generic;
using YAMP.Types;
using YAMP.Statements;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents the binary expression
    /// </summary>
    abstract class BinaryExpression : Expression
    {
        #region Members

        Expression _left;
        Expression _right;

        #endregion

        #region ctor

        BinaryExpression(Token token, Expression left, Expression right)
            : base(token)
        {
            _left = left ?? EmptyExpression.Instance;
            _right = right ?? EmptyExpression.Instance;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the left expression.
        /// </summary>
        public Expression Left
        {
            get { return _left; }
        }

        /// <summary>
        /// Gets the right expression.
        /// </summary>
        public Expression Right
        {
            get { return _right; }
        }

        #endregion

        #region Creator

        /// <summary>
        /// Creates a new binary expression.
        /// </summary>
        /// <param name="token">The token of the binary expression.</param>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>The binary expression.</returns>
        public static BinaryExpression Create(Token token, Expression left, Expression right)
        {
            switch (token.Category)
            {
                case TokenCategory.Assignment:
                    switch (token.Type)
                    {
                        case TokenType.Assignment:
                            return new AssignmentExpression(token, left, right);
                        case TokenType.Add:
                        case TokenType.Multiply:
                        case TokenType.Modulo:
                        case TokenType.LeftDivide:
                        case TokenType.Power:
                        case TokenType.RightDivide:
                        case TokenType.Subtract:
                            right = Create(Token.Operator(token.Type, token.Row, token.Column), left, right);
                            return new AssignmentExpression(token, left, right);
                    }
                    break;

                case TokenCategory.DotOperator:
                    switch (token.Type)
                    {
                        case TokenType.Multiply:
                            return new DotMulExpression(token, left, right);
                        case TokenType.LeftDivide:
                            return new DotLDivExpression(token, left, right);
                        case TokenType.RightDivide:
                            return new DotRDivExpression(token, left, right);
                        case TokenType.Power:
                            return new DotPowExpression(token, left, right);
                    }
                    break;

                case TokenCategory.Operator:
                    switch (token.Type)
                    {
                        case TokenType.Dot:
                            return new DotExpression(token, left, right);
                        case TokenType.Add:
                            return new AddExpression(token, left, right);
                        case TokenType.Subtract:
                            return new SubExpression(token, left, right);
                        case TokenType.Multiply:
                            return new MulExpression(token, left, right);
                        case TokenType.Modulo:
                            return new ModExpression(token, left, right);
                        case TokenType.LeftDivide:
                            return new LDivExpression(token, left, right);
                        case TokenType.RightDivide:
                            return new RDivExpression(token, left, right);
                        case TokenType.Power:
                            return new PowExpression(token, left, right);
                        case TokenType.LessEqual:
                            return new LteExpression(token, left, right);
                        case TokenType.LessThan:
                            return new LtExpression(token, left, right);
                        case TokenType.NotEqual:
                            return new NeqExpression(token, left, right);
                        case TokenType.Equal:
                            return new EqExpression(token, left, right);
                        case TokenType.GreaterEqual:
                            return new GteExpression(token, left, right);
                        case TokenType.GreaterThan:
                            return new GtExpression(token, left, right);
                        case TokenType.Or:
                            return new OrExpression(token, left, right);
                        case TokenType.And:
                            return new AndExpression(token, left, right);

                    }
                    break;
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
            return string.Format("({0}{1}{2})", _left.ToCode(), Token.ToCode(), _right.ToCode());
        }

        #endregion

        #region Nested expressions

        sealed class AddExpression : BinaryExpression
        {
            public AddExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return left + right;
            }
        }

        sealed class SubExpression : BinaryExpression
        {
            public SubExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return left - right;
            }
        }

        sealed class MulExpression : BinaryExpression
        {
            public MulExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return left * right;
            }
        }

        sealed class RDivExpression : BinaryExpression
        {
            public RDivExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return left / right;
            }
        }

        sealed class LDivExpression : BinaryExpression
        {
            public LDivExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return right / left;
            }
        }

        sealed class PowExpression : BinaryExpression
        {
            public PowExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return left ^ right;
            }
        }

        sealed class ModExpression : BinaryExpression
        {
            public ModExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return left % right;
            }
        }

        sealed class AssignmentExpression : BinaryExpression
        {
            public AssignmentExpression(Token token, Expression left, Expression right)
                : base(Token.Assignment(TokenType.Assignment, token.Row, token.Column), left, right)
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
                var value = _right.Evaluate(ctx);

                if (_left is VariableExpression)
                    ctx.SetVariable(_left.Token.ValueAsString, value);
                else if (_left is DotExpression)
                    ((DotExpression)_left).Evaluate(ctx, value);
                //else if (_left is FunctionExpression)
                //    return ((FunctionExpression)_left).Evaluate(ctx, value);
                else
                    throw new Exception("Only identifiers can be assigned.");

                return value;
            }
        }

        sealed class DotExpression : BinaryExpression
        {
            public DotExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
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
                var left = _left.Evaluate(ctx);

                if (left == null)
                    return null;

                var obj = left.Value is IObject ? (IObject)left.Value : new YObject(left.Value);

                if (_right is VariableExpression)
                {
                    var name = _right.Token.ValueAsString;

                    if (obj.HasProperty(name))
                    {
                        Dynamic value;

                        if (obj.TryReadProperty(name, out value))
                            return value;

                        throw new YException("The property " + name + " is write-only.");
                    }

                    throw new YException("The property {0} could not be found. The following properties are available: {1}.", name, Join(obj.Properties));
                }
                else if (_right is FunctionExpression)
                {
                    var name = ((FunctionExpression)_right).Expression.Token.ValueAsString;

                    if (obj.HasMethod(name))
                    {
                        var method = obj.GetMethod(name);
                        var args = (Dynamic[])(((FunctionExpression)_right).Arguments.Evaluate(ctx).Value);
                        var f = method.Resolve(args);

                        if (f == null)
                            throw new YException("The given parameters do not target any of these functions usages.");

                        return new Dynamic(f(args));
                    }

                    throw new YException(name, obj.Methods);
                }

                throw new YException("Only identifiers are acceptable.");
            }

            public void Evaluate(RunContext ctx, Dynamic value)
            {
                var left = _left.Evaluate(ctx);
                var obj = left.Value is IObject ? (IObject)left.Value : new YObject(left.Value);
                var name = _right.Token.ValueAsString;

                if (_right is VariableExpression)
                {
                    if (obj.HasProperty(name))
                    {
                        if (obj.TryWriteProperty(name, value))
                            return;

                        throw new YException("The property {0} is read-only.", name);
                    }

                    throw new YException("The property {0} could not be found. The following properties are available: {1}.", name, Join(obj.Properties));
                }
                
                throw new Exception("Only identifiers are acceptable.");
            }

            static string Join(string[] src)
            {
                if (src.Length == 0)
                    return "(none)";

                return string.Join(", ", src);
            }
        }

        sealed class DotMulExpression : BinaryExpression
        {
            public DotMulExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);

                if (left == null || right == null)
                    return null;

                if (left.Value is Matrix && right.Value is Matrix)
                {
                    var lm = (Matrix)left.Value;
                    var rm = (Matrix)right.Value;
                    var n = Math.Min(lm.Rows, rm.Rows);
                    var m = Math.Min(lm.Columns, rm.Columns);
                    var M = new Matrix(n, m);

                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                            M[j, i] = lm[j, i] * rm[j, i];
                    }

                    return new Dynamic(M);
                }
                else if (left.Value is Matrix)
                {
                    //var M = (Matrix)left.Value;
                    //var o = ComplexType.Instance.Cast(right);

                    //if (o != null)
                    //{
                    //    var c = (Complex)o;
                    //    return new Dynamic(M.ForEach(z => z * c));
                    //}
                }
                else if (right.Value is Matrix)
                {
                    //var M = (Matrix)right.Value;
                    //var o = ComplexType.Instance.Cast(left);

                    //if (o != null)
                    //{
                    //    var c = (Complex)o;
                    //    return new Dynamic(M.ForEach(z => z * c));
                    //}
                }

                return left * right;
            }
        }

        sealed class DotRDivExpression : BinaryExpression
        {
            public DotRDivExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);

                if (left == null || right == null)
                    return null;

                if (left.Value is Matrix && right.Value is Matrix)
                {
                    var lm = (Matrix)left.Value;
                    var rm = (Matrix)right.Value;
                    var n = Math.Min(lm.Rows, rm.Rows);
                    var m = Math.Min(lm.Columns, rm.Columns);
                    var M = new Matrix(n, m);

                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                            M[j, i] = lm[j, i] / rm[j, i];
                    }

                    return new Dynamic(M);
                }
                else if (left.Value is Matrix)
                {
                    //var M = (Matrix)left.Value;
                    //var o = ComplexType.Instance.Cast(right);

                    //if (o != null)
                    //{
                    //    var c = (Complex)o;
                    //    return new Dynamic(M.ForEach(z => z / c));
                    //}
                }
                else if (right.Value is Matrix)
                {
                    //var M = (Matrix)right.Value;
                    //var o = ComplexType.Instance.Cast(left);

                    //if (o != null)
                    //{
                    //    var c = (Complex)o;
                    //    return new Dynamic(M.ForEach(z => c / z));
                    //}
                }

                return left / right;
            }
        }

        sealed class DotLDivExpression : BinaryExpression
        {
            public DotLDivExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);

                if (left == null || right == null)
                    return null;

                if (left.Value is Matrix && right.Value is Matrix)
                {
                    var lm = (Matrix)left.Value;
                    var rm = (Matrix)right.Value;
                    var n = Math.Min(lm.Rows, rm.Rows);
                    var m = Math.Min(lm.Columns, rm.Columns);
                    var M = new Matrix(n, m);

                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                            M[j, i] = rm[j, i] / lm[j, i];
                    }

                    return new Dynamic(M);
                }
                else if (left.Value is Matrix)
                {
                    //var M = (Matrix)left.Value;
                    //var o = ComplexType.Instance.Cast(right);

                    //if (o != null)
                    //{
                    //    var c = (Complex)o;
                    //    return new Dynamic(M.ForEach(z => c / z));
                    //}
                }
                else if (right.Value is Matrix)
                {
                    //var M = (Matrix)right.Value;
                    //var o = ComplexType.Instance.Cast(left);

                    //if (o != null)
                    //{
                    //    var c = (Complex)o;
                    //    return new Dynamic(M.ForEach(z => z / c));
                    //}
                }

                return right / left;
            }
        }

        sealed class DotPowExpression : BinaryExpression
        {
            public DotPowExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
            {
            }

            /// <summary>
            /// Evaluates the given query in the given context.
            /// </summary>
            /// <param name="ctx">The context of the evaluation.</param>
            /// <returns>The result of the query.</returns>
            public override Dynamic Evaluate(RunContext ctx)
            {
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);

                if (left == null || right == null)
                    return null;

                if (left.Value is Matrix && right.Value is Matrix)
                {
                    var lm = (Matrix)left.Value;
                    var rm = (Matrix)right.Value;
                    var n = Math.Min(lm.Rows, rm.Rows);
                    var m = Math.Min(lm.Columns, rm.Columns);
                    var M = new Matrix(n, m);

                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                            M[j, i] = lm[j, i].Pow(rm[j, i]);
                    }

                    return new Dynamic(M);
                }
                else if (left.Value is Matrix)
                {
                    //var M = (Matrix)left.Value;
                    //var o = ComplexType.Instance.Cast(right);

                    //if (o != null)
                    //{
                    //    var c = (Complex)o;
                    //    return new Dynamic(M.ForEach(z => z.Pow(c)));
                    //}
                }
                else if (right.Value is Matrix)
                {
                    //var M = (Matrix)right.Value;
                    //var o = ComplexType.Instance.Cast(left);

                    //if (o != null)
                    //{
                    //    var c = (Complex)o;
                    //    return new Dynamic(M.ForEach(z => c.Pow(z)));
                    //}
                }

                return left ^ right;
            }
        }

        sealed class LteExpression : BinaryExpression
        {
            public LteExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
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
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return new Dynamic(left <= right);
            }
        }

        sealed class GteExpression : BinaryExpression
        {
            public GteExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
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
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return new Dynamic(left >= right);
            }
        }

        sealed class LtExpression : BinaryExpression
        {
            public LtExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
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
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return new Dynamic(left < right);
            }
        }

        sealed class GtExpression : BinaryExpression
        {
            public GtExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
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
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return new Dynamic(left > right);
            }
        }

        sealed class EqExpression : BinaryExpression
        {
            public EqExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
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
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return new Dynamic(left == right);
            }
        }

        sealed class NeqExpression : BinaryExpression
        {
            public NeqExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
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
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return new Dynamic(left != right);
            }
        }

        sealed class AndExpression : BinaryExpression
        {
            public AndExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
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
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return left && right;
            }
        }

        sealed class OrExpression : BinaryExpression
        {
            public OrExpression(Token token, Expression left, Expression right)
                : base(token, left, right)
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
                var left = _left.Evaluate(ctx);
                var right = _right.Evaluate(ctx);
                return left || right;
            }
        }

        #endregion
    }
}
