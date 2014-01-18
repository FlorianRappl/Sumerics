using System;
using System.Collections.Generic;
using YAMP.Parser;
using YAMP.Types;
using YAMP.Tables.Maps;
using YAMP.Core;

namespace YAMP.Tables
{
    class Mappings : Container, IAddBinary, IAddUnary, IAddLogic
    {
        #region Members

        LogicOperatorMapping equ;
        LogicOperatorMapping neq;
        LogicOperatorMapping gth;
        LogicOperatorMapping lth;
        LogicOperatorMapping gte;
        LogicOperatorMapping lte;

        BinaryOperatorMapping add;
        BinaryOperatorMapping sub;
        BinaryOperatorMapping mul;
        BinaryOperatorMapping div;
        BinaryOperatorMapping pwr;
        BinaryOperatorMapping mod;

        UnaryOperatorMapping neg;
        UnaryOperatorMapping trn;
        UnaryOperatorMapping adj;
        UnaryOperatorMapping fac;

        IType _source;
        Libraries _libs;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new mapping database.
        /// </summary>
        /// <param name="libs">The collection of libraries to consider.</param>
        public Mappings(Libraries libs)
        {
            _libs = libs;

            add = new BinaryOperatorMapping();
            sub = new BinaryOperatorMapping();
            mul = new BinaryOperatorMapping();
            div = new BinaryOperatorMapping();
            pwr = new BinaryOperatorMapping();
            mod = new BinaryOperatorMapping();

            neg = new UnaryOperatorMapping();
            trn = new UnaryOperatorMapping();
            adj = new UnaryOperatorMapping();
            fac = new UnaryOperatorMapping();

            equ = new LogicOperatorMapping();
            neq = new LogicOperatorMapping();
            lte = new LogicOperatorMapping();
            lth = new LogicOperatorMapping();
            gte = new LogicOperatorMapping();
            gth = new LogicOperatorMapping();
        }

        #endregion

        #region Resolvers

        /// <summary>
        /// Resolves the binary add operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveAddition(ref object left, ref object right)
        {
            return add.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary subtract operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveSubtraction(ref object left, ref object right)
        {
            return sub.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary multiplication operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveMultiplication(ref object left, ref object right)
        {
            return mul.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary division operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveDivision(ref object left, ref object right)
        {
            return div.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary modulo operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveModulo(ref object left, ref object right)
        {
            return mod.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary power operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolvePower(ref object left, ref object right)
        {
            return pwr.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary equal operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveEqual(ref object left, ref object right)
        {
            return equ.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary not equal operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveNotEqual(ref object left, ref object right)
        {
            return neq.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary greater or equal operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveGreaterEqual(ref object left, ref object right)
        {
            return gte.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary greater than operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveGreater(ref object left, ref object right)
        {
            return gth.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary less or equal operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveLessEqual(ref object left, ref object right)
        {
            return lte.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the binary less than operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object, object> ResolveLess(ref object left, ref object right)
        {
            return lth.Resolve(ref left, ref right);
        }

        /// <summary>
        /// Resolves the unary not operator.
        /// </summary>
        /// <param name="obj">The operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object> ResolveNot(ref object obj)
        {
            var td = _libs.Types.Find(obj);
            if (td == null) return o => o == null;
            return o => !td.IsTrue(o);
        }

        /// <summary>
        /// Resolves the unary factorial operator.
        /// </summary>
        /// <param name="obj">The operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object> ResolveFactorial(ref object obj)
        {
            return fac.Resolve(ref obj);
        }

        /// <summary>
        /// Resolves the unary transpose operator.
        /// </summary>
        /// <param name="obj">The operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object> ResolveTranspose(ref object obj)
        {
            return trn.Resolve(ref obj);
        }

        /// <summary>
        /// Resolves the unary adjungate operator.
        /// </summary>
        /// <param name="obj">The operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object> ResolveAdjungate(ref object obj)
        {
            return adj.Resolve(ref obj);
        }

        /// <summary>
        /// Resolves the unary negation operator.
        /// </summary>
        /// <param name="obj">The operand.</param>
        /// <returns>The corresponding function or null.</returns>
        public Func<object, object> ResolveNegate(ref object obj)
        {
            return neg.Resolve(ref obj);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers a new set of operators.
        /// </summary>
        /// <param name="source">The type source.</param>
        public void Register(IType source)
        {
            _source = source;
            _source = null;
            RaiseChanged("Operator", ChangeState.Added);
        }

        /// <summary>
        /// Unregisters a set of operators.
        /// </summary>
        /// <param name="source">The type source.</param>
        public void Unregister(IType source)
        {
            add.Remove(source);
            sub.Remove(source);
            mul.Remove(source);
            div.Remove(source);
            pwr.Remove(source);
            mod.Remove(source);
            neg.Remove(source);
            trn.Remove(source);
            adj.Remove(source);
            fac.Remove(source);
            gte.Remove(source);
            gth.Remove(source);
            lte.Remove(source);
            lth.Remove(source);
            equ.Remove(source);
            neq.Remove(source);
            RaiseChanged("Operator", ChangeState.Removed);
        }

        #endregion

        #region Register Binary

        void IAddBinary.Addition(Func<object, object, object> method, IType left, IType right)
        {
            add.Add(method, left, right, _source);
        }

        void IAddBinary.Subtract(Func<object, object, object> method, IType left, IType right)
        {
            sub.Add(method, left, right, _source);
        }

        void IAddBinary.Multiply(Func<object, object, object> method, IType left, IType right)
        {
            mul.Add(method, left, right, _source);
        }

        void IAddBinary.Division(Func<object, object, object> method, IType left, IType right)
        {
            div.Add(method, left, right, _source);
        }

        void IAddBinary.Power(Func<object, object, object> method, IType left, IType right)
        {
            pwr.Add(method, left, right, _source);
        }

        void IAddBinary.Modulo(Func<object, object, object> method, IType left, IType right)
        {
            mod.Add(method, left, right, _source);
        }

        #endregion

        #region Register Unary

        void IAddUnary.Negate(Func<object, object> method, IType operandType)
        {
            neg.Add(method, operandType, _source);
        }

        void IAddUnary.Factorial(Func<object, object> method, IType operandType)
        {
            fac.Add(method, operandType, _source);
        }

        void IAddUnary.Transpose(Func<object, object> method, IType operandType)
        {
            trn.Add(method, operandType, _source);
        }

        void IAddUnary.Adjungate(Func<object, object> method, IType operandType)
        {
            adj.Add(method, operandType, _source);
        }

        #endregion

        #region Register Logic

        void IAddLogic.Greater(Func<object, object, object> method, IType operandType)
        {
            gth.Add(method, operandType, _source);
        }

        void IAddLogic.GreaterEqual(Func<object, object, object> method, IType operandType)
        {
            gte.Add(method, operandType, _source);
        }

        void IAddLogic.Less(Func<object, object, object> method, IType operandType)
        {
            lth.Add(method, operandType, _source);
        }

        void IAddLogic.LessEqual(Func<object, object, object> method, IType operandType)
        {
            lte.Add(method, operandType, _source);
        }

        void IAddLogic.Equal(Func<object, object, object> method, IType operandType)
        {
            equ.Add(method, operandType, _source);
        }

        void IAddLogic.NotEqual(Func<object, object, object> method, IType operandType)
        {
            neq.Add(method, operandType, _source);
        }

        #endregion
    }
}
