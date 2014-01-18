using System;
using System.Collections;
using System.Collections.Generic;
using YAMP.Core;
using YAMP.Parser;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents an array expression.
    /// </summary>
    sealed class ArrayExpression : Expression, IEnumerable<Expression>
    {
        #region Members

        List<Expression> _expressions;

        #endregion

        #region ctor

        public ArrayExpression(Token token)
            : base(token)
        {
            _expressions = new List<Expression>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the length of the list.
        /// </summary>
        public Int32 Length
        {
            get { return _expressions.Count; }
        }

        /// <summary>
        /// Gets a specific expression from the list.
        /// </summary>
        /// <param name="index">The 0-based index.</param>
        /// <returns>The expression.</returns>
        public Expression this[Int32 index]
        {
            get { return _expressions[index]; }
        }

        #endregion

        #region Methods

        public override Dynamic Evaluate(RunContext ctx)
        {
            var array = new DynamicList();

            for (int i = 0; i < _expressions.Count; i++)
                array.Add(_expressions[i].Evaluate(ctx));

            return new Dynamic(array);
        }

        public void Add(Expression expression)
        {
            _expressions.Add(expression);
        }

        public IEnumerator<Expression> GetEnumerator()
        {
            return _expressions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_expressions).GetEnumerator();
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override String ToCode()
        {
            var entries = new String[Length];

            for (int i = 0; i < Length; i++)
                entries[i] = _expressions[i].ToCode();

            return "[" + String.Join(", ", entries) + "]";
        }

        #endregion
    }
}
