using System;
using System.Collections.Generic;
using YAMP.Parser;
using YAMP.Core;
using YAMP.Types;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents a ternary expression.
    /// </summary>
    abstract class TernaryExpression : Expression
    {
        #region Members

        Expression _left;
        Expression _middle;
        Expression _right;

        #endregion

        #region ctor

        public TernaryExpression(Token token, Expression left, Expression middle, Expression right)
            : base(token)
        {
            _left = left ?? EmptyExpression.Instance;
            _middle = middle ?? EmptyExpression.Instance;
            _right = right ?? EmptyExpression.Instance;
        }

        #endregion

        #region Properties

        public Expression Left
        {
            get { return _left; }
        }

        public Expression Middle
        {
            get { return _middle; }
        }

        public Expression Right
        {
            get { return _right; }
        }

        #endregion

        #region Creator

        /// <summary>
        /// Creates a new ternary expression.
        /// </summary>
        /// <param name="token">The token of the ternary expression.</param>
        /// <param name="left">The left (condition or start) expression.</param>
        /// <param name="middle">The middle (true or step) expression.</param>
        /// <param name="right">The right (false or end) expression.</param>
        /// <returns>The ternary expression.</returns>
        public static TernaryExpression Create(Token token, Expression left, Expression middle, Expression right)
        {
            switch (token.Type)
            {
                case TokenType.Condition:
                    return new ConditionalExpression(token, left, middle, right);
                case TokenType.Range:
                    return new RangeExpression(token, left, middle, right);
            }

            return null;
        }

        #endregion

        #region Nested

        sealed class RangeExpression : TernaryExpression
        {
            bool _implicit;

            public RangeExpression(Token token, Expression start, Expression step, Expression end)
                : base(token, start, step, end)
            {
                _implicit = step == null;
            }

            public override Dynamic Evaluate(RunContext ctx)
            {
                var beg = _left.Evaluate(ctx);
                var end = _right.Evaluate(ctx);
                return new Dynamic();
                //if (_implicit)
                //    return CR(beg, end);

                //var stp = _middle.Evaluate(ctx);
                //return CR(beg, stp, end);
            }

            //Dynamic CR(Dynamic beg, Dynamic end)
            //{
            //    if ((IntegerType.Instance.IsDirect(beg) || IntegerType.Instance.IsIndirect(beg)) && 
            //        (IntegerType.Instance.IsDirect(end) || IntegerType.Instance.IsIndirect(end)))
            //    {
            //        var i = (Int64)IntegerType.Instance.Cast(beg);
            //        var f = (Int64)IntegerType.Instance.Cast(end);
            //        return new Dynamic(Range.Create(i, f).ToArray());
            //    }
            //    else
            //    {
            //        var i = RealType.Instance.Cast(beg);
            //        var f = RealType.Instance.Cast(end);

            //        if (i == null || f == null)
            //            throw new YException("Only real or integer values can be used for constructing ranges.");

            //        var ir = (Double)i;
            //        var fr = (Double)f;
            //        return new Dynamic(Range.Create(ir, fr).ToArray());
            //    }
            //}

            //Dynamic CR(Dynamic beg, Dynamic stp, Dynamic end)
            //{
            //    if ((IntegerType.Instance.IsDirect(beg) || IntegerType.Instance.IsIndirect(beg)) &&
            //        (IntegerType.Instance.IsDirect(end) || IntegerType.Instance.IsIndirect(end)) &&
            //        (IntegerType.Instance.IsDirect(stp) || IntegerType.Instance.IsIndirect(stp)))
            //    {
            //        var i = (Int64)IntegerType.Instance.Cast(beg);
            //        var f = (Int64)IntegerType.Instance.Cast(end);
            //        var s = (Int64)IntegerType.Instance.Cast(stp);
            //        return new Dynamic(Range.Create(i, f, s).ToArray());
            //    }
            //    else
            //    {
            //        var i = RealType.Instance.Cast(beg);
            //        var f = RealType.Instance.Cast(end);
            //        var s = RealType.Instance.Cast(stp);

            //        if (i == null || f == null || s == null)
            //            throw new YException("Only real or integer values can be used for constructing ranges.");

            //        var ir = (Double)i;
            //        var fr = (Double)f;
            //        var sr = (Double)s;
            //        return new Dynamic(Range.Create(ir, fr, sr).ToArray());
            //    }
            //}

            public override string ToCode()
            {
                if (_implicit)
                    return string.Format("({0}:{1})", _left.ToCode(), _right.ToCode());

                return string.Format("({0}:{1}:{2})", _left.ToCode(), _middle.ToCode(), _right.ToCode());
            }
        }

        sealed class ConditionalExpression : TernaryExpression
        {
            public ConditionalExpression(Token token, Expression condition, Expression iftrue, Expression iffalse)
                : base(token, condition, iftrue, iffalse)
            {
            }

            public override Dynamic Evaluate(RunContext ctx)
            {
                if (_left.Evaluate(ctx))
                    return _middle.Evaluate(ctx);
                else
                    return _right.Evaluate(ctx);
            }

            public override string ToCode()
            {
                return string.Format("({0}?{1}:{2})", _left.ToCode(), _middle.ToCode(), _right.ToCode());
            }
        }

        #endregion
    }
}
