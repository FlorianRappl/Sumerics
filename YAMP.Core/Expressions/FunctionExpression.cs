using System;
using System.Collections.Generic;
using YAMP.Parser;
using YAMP.Core;
using YAMP.Types;
using System.Collections;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents a function (call) expression.
    /// </summary>
    class FunctionExpression : Expression
    {
        #region Members

        ListExpression _arguments;
        Expression _exp;

        #endregion

        #region ctor

        public FunctionExpression(Token start, Expression exp, ListExpression arguments)
            : base(start)
        {
            _exp = exp;
            _arguments = arguments ?? new ListExpression(start);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the arguments of the function call.
        /// </summary>
        public ListExpression Arguments 
        {
            get { return _arguments; }
        }

        /// <summary>
        /// Gets the expression of the function call.
        /// </summary>
        public Expression Expression
        {
            get { return _exp; }
        }

        #endregion

        #region Methods

        public override Dynamic Evaluate(RunContext ctx)
        {
            var f = _exp.Evaluate(ctx);
            var args = (Dynamic[])_arguments.Evaluate(ctx).Value;
            return f.Call(args);
        }

        //public object Evaluate(RunContext ctx, object value)
        //{
        //    var var = _exp.Evaluate(ctx);

        //    if (var is Matrix)
        //        return Evaluate(ctx, (Matrix)var, value);
        //    else if (var is Array)
        //        return Evaluate(ctx, (Array)var, value);

        //    throw new YException("Only matrices and arrays can be adjusted with an assigning function call.");
        //}

        //public Dynamic Evaluate(RunContext ctx, Int32 rows, Int32 columns)
        //{
        //    var o = new Dynamic[_expressions.Count];

        //    if (_expressions.Count == 1)
        //    {
        //        var scope = new Scope();
        //        scope.Set("end", new Dynamic(rows * columns - 1));
        //        ctx.CreateScope(scope);
        //        o[0] = _expressions[0].Evaluate(ctx);
        //        ctx.ReleaseScope();
        //    }
        //    else if (_expressions.Count == 2)
        //    {
        //        var scope = new Scope();
        //        ctx.CreateScope(scope);
        //        scope.Set("end", new Dynamic(rows - 1));
        //        o[0] = _expressions[0].Evaluate(ctx);
        //        scope.Set("end", new Dynamic(columns - 1));
        //        o[1] = _expressions[1].Evaluate(ctx);
        //        ctx.ReleaseScope();
        //    }
        //    else
        //    {
        //        for (int i = 0; i < _expressions.Count; i++)
        //            o[i] = _expressions[i].Evaluate(ctx);
        //    }

        //    return new Dynamic(o);
        //}

        #endregion

        #region Helpers

        //object Evaluate(RunContext ctx, Matrix m)
        //{
        //    var fdef = new MatrixFunction(m);
        //    var args = _arguments.Evaluate(ctx, m.Rows, m.Columns);

        //    if (args.Length == 0 || args.Length > 2)
        //        throw new YException("Matrices can only be accessed with one or two indices.");

        //    var f = fdef.Resolve(args);

        //    if (f == null)
        //        throw new YException("The given parameters cannot be used as matrix indices.");

        //    return f(ctx, args);
        //}

        //object Evaluate(RunContext ctx, Array a)
        //{
        //    var args = _arguments.Evaluate(ctx, 1, a.Length);
        //    var fdef = new ArrayFunction(a);

        //    if (args.Length != 1)
        //        throw new YException("Arrays can only be accessed with one index.");

        //    var f = fdef.Resolve(args);

        //    if (f == null)
        //        throw new YException("The given parameter cannot be used as array index.");

        //    return f(ctx, args);
        //}

        //object Evaluate(RunContext ctx, Matrix m, object value)
        //{
        //    var fdef = new MatrixFunction(m);
        //    var args = _arguments.Evaluate(ctx, m.Rows, m.Columns);
        //    var f = fdef.Resolve(args, ref value);

        //    if (f == null)
        //        throw new YException("The given parameters do not target any of these functions usages.");

        //    f(ctx, args, value);
        //    return m;
        //}

        //object Evaluate(RunContext ctx, Array a, object value)
        //{
        //    var fdef = new ArrayFunction(a);
        //    var args = _arguments.Evaluate(ctx, 1, a.Length);
        //    var f = fdef.Resolve(args, ref value);

        //    if (f == null)
        //        throw new YException("The given parameters do not target any of these functions usages.");

        //    f(ctx, args, value);
        //    return a;
        //}

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override String ToCode()
        {
            return _exp.ToCode() + _arguments.ToCode();
        }

        #endregion

        //static class IndexFunction
        //{
        //    struct Index
        //    {
        //        public int Row;
        //        public int Column;
        //    }

        //    static readonly ArrayType intArray = new ArrayType(IntegerType.Instance);

        //    /*
        //    public Action<RunContext, object[], object> Resolve(object[] parameters, ref object value)
        //    {
        //        if (ComplexType.Instance.IsDirect(value) || ComplexType.Instance.IsIndirect(value))
        //            value = ComplexType.Instance.Cast(value);
        //        else if (MatrixType.Instance.IsDirect(value) || MatrixType.Instance.IsIndirect(value))
        //            value = MatrixType.Instance.Cast(value);
        //        else
        //            return null;

        //        if (parameters.Length == 1)
        //        {
        //            var p = parameters[0];

        //            if (IntegerType.Instance.IsDirect(p) || IntegerType.Instance.IsIndirect(p))
        //                parameters[0] = IntegerType.Instance.Cast(p);
        //            else if (MatrixType.Instance.IsDirect(p) || MatrixType.Instance.IsIndirect(p))
        //                parameters[0] = (Matrix)MatrixType.Instance.Cast(p);
        //            else
        //                return null;
        //        }
        //        else if (parameters.Length == 2)
        //        {
        //            var p1 = parameters[0];
        //            var p2 = parameters[1];

        //            if ((IntegerType.Instance.IsDirect(p1) || IntegerType.Instance.IsIndirect(p1)) && (IntegerType.Instance.IsDirect(p2) || IntegerType.Instance.IsIndirect(p2)))
        //            {
        //                parameters[0] = IntegerType.Instance.Cast(p1);
        //                parameters[1] = IntegerType.Instance.Cast(p2);
        //            }
        //            else if (intArray.IsDirect(p1) && intArray.IsDirect(p2))
        //            {
        //                parameters[0] = intArray.Cast(p1);
        //                parameters[1] = intArray.Cast(p2);
        //            }
        //            else if (intArray.IsDirect(p1) && (IntegerType.Instance.IsDirect(p2) || IntegerType.Instance.IsIndirect(p2)))
        //            {
        //                parameters[0] = intArray.Cast(p1);
        //                parameters[1] = new Int64[] { (Int64)IntegerType.Instance.Cast(p2) };
        //            }
        //            else if ((IntegerType.Instance.IsDirect(p1) || IntegerType.Instance.IsIndirect(p1)) && intArray.IsDirect(p2))
        //            {
        //                parameters[0] = new Int64[] { (Int64)IntegerType.Instance.Cast(p1) };
        //                parameters[1] = intArray.Cast(p2);
        //            }
        //            else
        //                return null;
        //        }
        //        else
        //            return null;

        //        return (ctx, args, v) => m.Set(args, v);
        //    }
        //    */

        //    public static object Eval(RunContext ctx, Matrix m, ListExpression args)
        //    {
        //        var param = (object[])args.Evaluate(ctx, m.Rows, m.Columns).Value;
        //        var num = InspectParameters(param);
        //        return null;
        //    }

        //    public static object Eval(RunContext ctx, Array a, ListExpression args)
        //    {
        //        var param = (object[])args.Evaluate(ctx, 1, a.Length).Value;
        //        var num = InspectParameters(param);
        //        return null;
        //    }

        //    static Index[,] InspectParameters(object[] param)
        //    {
        //        if (param.Length == 0)
        //            throw new YException("At least one index has to be given.");
        //        else if (param.Length > 2)
        //            throw new YException("More than two indices are not allowed.");

        //        var dim = new int[2];

        //        for (int i = 0; i < param.Length; i++)
        //        {
        //            if (!intArray.IsDirect(param[i]))
        //            {
        //                if (IntegerType.Instance.IsDirect(param[i]) || IntegerType.Instance.IsIndirect(param[i]))
        //                    param[i] = new Int64[] { (Int64)IntegerType.Instance.Cast(param[i]) };
        //                else if (MatrixType.Instance.IsDirect(param[i]) || MatrixType.Instance.IsIndirect(param[i]))
        //                    param[i] = ((Matrix)MatrixType.Instance.Cast(param[i])).ToIntegerArray();
        //                else
        //                    throw new YException("Can only use integers as indices.");
        //            }

        //            dim[i] = ((Int64[])param[i]).Length;
        //        }

        //        if (param.Length == 1)
        //        {
        //            dim[1] = dim[0];
        //            dim[0] = 1;
        //        }

        //        var indices = new Index[dim[0], dim[1]];

        //        for (int i = 0; i < dim[0]; i++)
        //        {
        //            for (int j = 0; j < dim[1]; j++)
        //            {
                        
        //            }
        //        }

        //        return indices;
        //    }
        //}

    }
}
