using System;
using YAMP.Parser;
using YAMP.Core;
using YAMP.Types;
using System.Collections.Generic;
using System.Collections;

namespace YAMP.Expressions
{
    /// <summary>
    /// Represents a matrix expression.
    /// </summary>
    sealed class MatrixExpression : Expression, IEnumerable<Expression>
    {
        #region Members

        List<List<Expression>> _expressions;

        #endregion

        #region ctor

        public MatrixExpression(Token token)
            : base(token)
        {
            _expressions = new List<List<Expression>>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        public Int32 Rows
        {
            get { return _expressions.Count; }
        }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public Int32 Columns
        {
            get 
            {
                var max = 0;

                foreach (var row in _expressions)
                    if (row.Count > max)
                        max = row.Count;

                return max;
            }
        }

        /// <summary>
        /// Gets the number of expressions.
        /// </summary>
        public Int32 Count
        {
            get
            {
                var sum = 0;

                foreach (var row in _expressions)
                    sum += row.Count;

                return sum;
            }
        }

        /// <summary>
        /// Gets a specific expression from the list.
        /// </summary>
        /// <param name="row">The 0-based row index.</param>
        /// <param name="col">The 0-based col index.</param>
        /// <returns>The expression.</returns>
        public Expression this[Int32 row, Int32 col]
        {
            get { return _expressions[row][col]; }
        }

        /// <summary>
        /// Gets a list expression of the row from the list.
        /// </summary>
        /// <param name="row">The 0-based row index.</param>
        /// <returns>The expression.</returns>
        public ListExpression this[Int32 row]
        {
            get 
            {
                var list = new ListExpression(_token);
                var entries = _expressions[row];

                for (int i = 0; i < entries.Count; i++)
                    list.Add(entries[i]);

                return list;
            }
        }

        #endregion

        #region Methods

        public override Dynamic Evaluate(RunContext ctx)
        {
            var M = new Matrix(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                var entries = _expressions[i];

                for (int j = 0; j < entries.Count; j++)
                {
                    var item = entries[j].Evaluate(ctx);
                    M[i, j] = item.ConvertTo<Complex>();
                }
            }

            return new Dynamic(M);
        }

        public void AddRow()
        {
            _expressions.Add(new List<Expression>());
        }

        public void AddExpression(Expression expression)
        {
            _expressions[_expressions.Count - 1].Add(expression);
        }

        public IEnumerator<Expression> GetEnumerator()
        {
            foreach (var row in _expressions)
                foreach (var col in row)
                    yield return col;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Transforms the expression into code.
        /// </summary>
        /// <returns>The code that represents the statement.</returns>
        public override string ToCode()
        {
            var rows = new String[Rows];
            var columns = new String[Columns];

            for (int i = 0; i < Rows; i++)
            {
                var entries = _expressions[i];

                for (int j = 0; j < Columns; j++)
                    columns[j] = entries.Count > j ? entries[j].ToCode() : "0";

                rows[i] = string.Join(", ", columns);
            }

            return "<<" + string.Join("; ", rows) + ">>";
        }

        #endregion
    }
}
