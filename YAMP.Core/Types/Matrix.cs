/*
	Copyright (c) 2012-2013, Florian Rappl et al.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
		  may be used to endorse or promote products derived from this
		  software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP
{
    /// <summary>
    /// Represents a complex matrix.
    /// </summary>
    [Serializable]
    public class Matrix : IComparable, IList, IFormattable, IComparable<Matrix>, IEquatable<Matrix>
    {
        #region Members

        const int BLOCK = 8;

        Dictionary<Index, Complex[]> blocks;
        int rows;
        int columns;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new empty matrix.
        /// </summary>
        public Matrix()
            : this(0, 0)
        {
        }

        /// <summary>
        /// Creates a new empty matrix with the given dimensions.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(int rows, int columns)
        {
            this.blocks = new Dictionary<Index, Complex[]>();
            this.rows = rows;
            this.columns = columns;
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 2D jagged real array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Double[][] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0; k < rows; k++)
            {
                for (int i = 0; i < columns; i++)
                    this[k, i] = values[k][i];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 2D real array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Double[,] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0; k < rows; k++)
            {
                for (int i = 0; i < columns; i++)
                    this[k, i] = values[k, i];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 1D real array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Double[] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0, j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++, k++)
                    this[j, i] = values[k];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 1D integer array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Int64[] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0, j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++, k++)
                    this[j, i] = values[k];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 1D integer array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Int32[] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0, j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++, k++)
                    this[j, i] = values[k];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 1D integer array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Int16[] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0, j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++, k++)
                    this[j, i] = values[k];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 1D real array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Single[] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0, j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++, k++)
                    this[j, i] = values[k];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 1D integer array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Char[] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0, j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++, k++)
                    this[j, i] = values[k];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 2D jagged complex array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Complex[][] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                    this[j, i] = values[j][i];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 2D complex array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Complex[,] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0; k < rows; k++)
            {
                for (int i = 0; i < columns; i++)
                    this[k, i] = values[k, i];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given dimensions and values.
        /// </summary>
        /// <param name="values">A 1D complex array.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix(Complex[] values, int rows, int columns)
            : this(rows, columns)
        {
            for (int k = 0, j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++, k++)
                    this[j, i] = values[k];
            }
        }

        /// <summary>
        /// Creates a new matrix with the given values.
        /// </summary>
        /// <param name="values">A 1D real array.</param>
        public Matrix(Double[] values)
            : this(values, 1, values.Length)
        {
        }

        /// <summary>
        /// Creates a new matrix with the given values.
        /// </summary>
        /// <param name="values">A 1D integers array.</param>
        public Matrix(Int64[] values)
            : this(values, 1, values.Length)
        {
        }

        /// <summary>
        /// Creates a new matrix with the given values.
        /// </summary>
        /// <param name="values">A 1D integer array.</param>
        public Matrix(Int32[] values)
            : this(values, 1, values.Length)
        {
        }

        /// <summary>
        /// Creates a new matrix with the given values.
        /// </summary>
        /// <param name="values">A 1D complex array.</param>
        public Matrix(Complex[] values)
            : this(values, 1, values.Length)
        {
        }

        /// <summary>
        /// Creates a new matrix with the given values.
        /// </summary>
        /// <param name="values">A 1D real array.</param>
        public Matrix(Single[] values)
            : this(values, 1, values.Length)
        {
        }

        /// <summary>
        /// Creates a new matrix with the given values.
        /// </summary>
        /// <param name="values">A 1D integer array.</param>
        public Matrix(Char[] values)
            : this(values, 1, values.Length)
        {
        }

        #endregion

        #region Static creators

        /// <summary>
        /// Creates a new identity matrix of the given dimension.
        /// </summary>
        /// <param name="dimension">The rank of the identity matrix.</param>
        /// <returns>A new identity matrix.</returns>
        public static Matrix One(int dimension)
        {
            var M = new Matrix(dimension, dimension);

            for (int i = 0; i < dimension; i++)
                M[i, i] = Complex.One;

            return M;
        }

        /// <summary>
        /// Creates a new identity matrix of the given dimension.
        /// </summary>
        /// <param name="rows">The number of rows of the identity matrix.</param>
        /// <param name="columns">The number of columns of the identity matrix.</param>
        /// <returns>A new identity matrix.</returns>
        public static Matrix One(int rows, int columns)
        {
            var m = new Matrix(rows, columns);
            var dim = Math.Min(rows, columns);

            for (var i = 0; i < dim; i++)
                m[i, i] = Complex.One;

            return m;
        }

        /// <summary>
        /// Creates a matrix containing only ones.
        /// </summary>
        /// <param name="rows">The number of rows in the new matrix.</param>
        /// <param name="cols">The number of columns in the new matrix.</param>
        /// <returns>A new matrix containing only ones.</returns>
        public static Matrix Ones(int rows, int cols)
        {
            var m = new Matrix(rows, cols);

            for (var j = 0; j < rows; j++)
            {
                for (var i = 0; i < cols; i++)
                    m[j, i] = Complex.One;
            }

            return m;
        }

        /// <summary>
        /// Creates a matrix containing only zeros.
        /// </summary>
        /// <param name="rows">The number of rows in the new matrix.</param>
        /// <param name="cols">The number of columns in the new matrix.</param>
        /// <returns>A new matrix containing only zeros.</returns>
        public static Matrix Zeros(int rows, int cols)
        {
            var m = new Matrix(rows, cols);
            return m;
        }

        #endregion

        #region Index

        /// <summary>
        /// Gets or sets the i-th element of the matrix (counted rows-first).
        /// </summary>
        /// <param name="i">The 0-based index.</param>
        /// <returns>The value in the specified cell.</returns>
        public Complex this[int k]
        {
            get
            {
                if (k >= Length || k < 0)
                    throw new YException("Access out of bounds. The index has to be between 0 and {0}.", Length - 1);

                var length = Math.Max(1, rows);
                var col = k / length;
                var row = k - col * length;
                return this[row, col];
            }
            set
            {
                if (k < 0)
                    throw new YException("Access out of bounds. The index has to be greater or equal to 0.");

                var length = Math.Max(1, rows);
                var col = k / length;
                var row = k - col * length;
                this[row, col] = value;
            }
        }

        /// <summary>
        /// Gets or sets the entry in the specified row and column.
        /// </summary>
        /// <param name="j">The 0-based row index.</param>
        /// <param name="i">The 0-based column index.</param>
        /// <returns>The value in the specified cell.</returns>
        public Complex this[int j, int i]
        {
            get 
            {
                if (j < 0 || j >= Rows)
                    throw new YException("Access out of bounds. The row index has to be between 0 and {0}.", Rows - 1);
                else if (i < 0 || i >= Columns)
                    throw new YException("Access out of bounds. The column index has to be between 0 and {0}.", Columns - 1);

                var bj = j / BLOCK;
                var bi = i / BLOCK;
                var idx = new Index { j = bj, i = bi };
                Complex[] block;

                if (blocks.TryGetValue(idx, out block))
                {
                    j = j - bj * BLOCK;
                    i = i - bi * BLOCK;
                    return block[j * BLOCK + i];
                }

                return Complex.Zero; 
            }
            set
            {
                if (j < 0)
                    throw new YException("Access out of bounds. The row index has to be greater or equal to 0.");
                else if (i < 0)
                    throw new YException("Access out of bounds. The column index has to be greater or equal to 0.");

                if (j > rows) rows = j;
                if (i > columns) columns = i;
                
                var bj = j / BLOCK;
                var bi = i / BLOCK;
                var idx = new Index { j = bj, i = bi };
                Complex[] block;
                j = j - bj * BLOCK;
                i = i - bi * BLOCK;

                if (!blocks.TryGetValue(idx, out block))
                {
                    if (value == 0.0)
                        return;

                    block = new Complex[BLOCK * BLOCK];
                    blocks.Add(idx, block);
                }

                block[j * BLOCK + i] = value;
            }
        }

        /// <summary>
        /// Used to mark indices.
        /// </summary>
        struct Index
        {
            /// <summary>
            /// Determines the block column.
            /// </summary>
            public int i;

            /// <summary>
            /// Determines the block row.
            /// </summary>
            public int j;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the matrix is square, i.e. cols == rows.
        /// </summary>
        public bool IsSquare
        {
            get { return columns == rows; }
        }

        /// <summary>
        /// Gets if the matrix is actually a vector.
        /// </summary>
        public bool IsVector
        {
            get
            {
                if (rows != 1 && columns != 1)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Gets if the matrix has any non-zero elements.
        /// </summary>
        public bool HasElements
        {
            get
            {
                for (var j = 0; j < rows; j++)
                    for (var i = 0; i < columns; i++)
                        if (this[i, j] != Complex.Zero)
                            return true;

                return false;
            }
        }

        /// <summary>
        /// Gets a boolean if the matrix has any complex (im != 0.0) entries.
        /// </summary>
        public bool IsComplex
        {
            get
            {
                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < columns; j++)
                    {
                        if (Math.Abs(this[i, j].Im) > double.Epsilon)
                            return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value if the matrix is symmetric, i.e. M_ij = M_ji
        /// </summary>
        public bool IsSymmetric
        {
            get
            {
                if (columns != rows)
                    return false;

                for (var i = 0; i < columns; i++)
                {
                    for (var j = 0; j < i; j++)
                    {
                        if (this[i, j] != this[j, i])
                            return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value if the matrix is hermitian, i.e. M_ij = M_ji*
        /// </summary>
        public bool IsHermitian
        {
            get
            {
                if (columns != rows)
                    return false;

                for (var i = 0; i < columns; i++)
                {
                    for (var j = 0; j < i; j++)
                    {
                        if (this[i, j].Re != this[j, i].Re || this[i, j].Im != -this[j, i].Im)
                            return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the length of the matrix, i.e. rows * columns.
        /// </summary>
        public int Length
        {
            get { return columns * rows; }
        }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int Columns
        {
            get { return columns; }
        }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        public int Rows
        {
            get { return rows; }
        }

        #endregion

        #region Special Matrix operations

        /// <summary>
        /// Performs logical subscripting on the matrix.
        /// </summary>
        /// <param name="m">The logical matrix (indices).</param>
        /// <returns>The matrix consisting of the values.</returns>
        public Matrix LogicalSubscripting(Matrix m)
        {
            var indices = new List<Index>();
            LogicalSubscripting(m, indices);
            var n = new Matrix(1, indices.Count);

            for (var i = 0; i < indices.Count; i++)
            {
                var index = indices[i];
                n[0, i] = this[index.j, index.i];
            }

            return n;
        }

        /// <summary>
        /// Clears all values but leaves the dimensions unchanged.
        /// </summary>
        public void Clear()
        {
            blocks.Clear();
        }

        /// <summary>
        /// Compares two matrices for equality (element-wise).
        /// </summary>
        /// <param name="A">The given matrix.</param>
        /// <param name="B">The matrix to compare with.</param>
        /// <returns>A logical matrix.</returns>
        public static Matrix CompareEquality(Matrix A, Matrix B)
        {
            var M = new Matrix(Math.Max(A.rows, B.rows), Math.Max(A.columns, B.columns));
            var n = Math.Min(A.rows, B.rows);
            var m = Math.Min(A.columns, B.columns);

            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < m; i++)
                    M[j, i] = A[j, i] == B[j, i] ? 1 : 0;
            }

            return M;
        }

        /// <summary>
        /// Compares two matrices for inequality (element-wise).
        /// </summary>
        /// <param name="A">The given matrix.</param>
        /// <param name="B">The matrix to compare with.</param>
        /// <returns>A logical matrix.</returns>
        public static Matrix CompareInequality(Matrix A, Matrix B)
        {
            var M = new Matrix(Math.Max(A.rows, B.rows), Math.Max(A.columns, B.columns));
            var n = Math.Min(A.rows, B.rows);
            var m = Math.Min(A.columns, B.columns);

            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < m; i++)
                    M[j, i] = A[j, i] != B[j, i] ? 1 : 0;
            }

            return M;
        }

        /// <summary>
        /// Creates a copy of the given matrix.
        /// </summary>
        public Matrix Copy()
        {
            var M = new Matrix(rows, columns);
            
			for (int j = 0; j < rows; j++)
			{
                for (int i = 0; i < columns; i++)
                    M[j, i] = this[j, i];
			}

            return M;
        }

        /// <summary>
        /// Copies the values to a 1-dim. complex array.
        /// </summary>
        public Complex[] ToArray()
        {
            var array = new Complex[Length];

            foreach (var block in blocks)
            {
                var oi = block.Key.i * BLOCK;
                var oj = block.Key.j * BLOCK;
                var r = Math.Min(rows, oj + BLOCK);
                var c = Math.Min(columns, oi+ BLOCK);

                for (int j = oj, l = 0; j < r; j++, l++)
                {
                    var k = j * columns;
                    var m = l * BLOCK;

                    for (int i = oi; i < c; i++, k++, m++)
                        array[k] = block.Value[m];
                }
            }

            return array;
        }

        /// <summary>
        /// Copies the values to a 1-dim. real array.
        /// </summary>
        public Double[] ToRealArray()
        {
            var array = new Double[Length];

            foreach (var block in blocks)
            {
                var oi = block.Key.i * BLOCK;
                var oj = block.Key.j * BLOCK;
                var r = Math.Min(rows, oj + BLOCK);
                var c = Math.Min(columns, oi + BLOCK);

                for (int j = oj, l = 0; j < r; j++, l++)
                {
                    var k = j * columns;
                    var m = l * BLOCK;

                    for (int i = oi; i < c; i++, k++, m++)
                        array[k] = block.Value[m].Re;
                }
            }

            return array;
        }

        /// <summary>
        /// Copies the values to a 1-dim. integer array.
        /// </summary>
        public Int64[] ToIntegerArray()
        {
            var array = new Int64[Length];

            foreach (var block in blocks)
            {
                var oi = block.Key.i * BLOCK;
                var oj = block.Key.j * BLOCK;
                var r = Math.Min(rows, oj + BLOCK);
                var c = Math.Min(columns, oi + BLOCK);

                for (int j = oj, l = 0; j < r; j++, l++)
                {
                    var k = j * columns;
                    var m = l * BLOCK;

                    for (int i = oi; i < c; i++, k++, m++)
                        array[k] = (Int64)block.Value[m].Re;
                }
            }

            return array;
        }

        /// <summary>
        /// Computes the inverse (if it exists).
        /// </summary>
        /// <returns>The inverse matrix.</returns>
        public Matrix Inv()
        {
            var target = One(columns);

            if (columns < 24)
            {
                var lu = new LUDecomposition(this);
                return lu.Solve(target);
            }
            else if (IsSymmetric)
            {
                var cho = new CholeskyDecomposition(this);
                return cho.Solve(target);
            }

            var qr = QRDecomposition.Create(this);
            return qr.Solve(target);
        }

        /// <summary>
        /// Computes the adjungated (transposed + c.c.) matrix.
        /// </summary>
        /// <returns>The adjungated matrix.</returns>
        public Matrix Adj()
        {
            var M = new Matrix(columns, rows);

            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                    M[i, j] = this[j, i].Conj();
            }

            return M;
        }

        /// <summary>
        /// Computes the transposed matrix.
        /// </summary>
        /// <returns>The transposed matrix.</returns>
        public Matrix Trans()
        {
            var M = new Matrix(columns, rows);

            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                    M[i, j] = this[j, i];
            }

            return M;
        }

        /// <summary>
        /// Computes the trace (sum over all elements on the diagonal) of the matrix.
        /// </summary>
        /// <returns>The value of the computation.</returns>
        public Complex Trace()
        {
            var sum = Complex.Zero;
            var n = Math.Min(rows, columns);

            for (var i = 0; i < n; i++)
                sum += this[i, i];

            return sum;
        }

        /// <summary>
        /// Computes the determinant of the matrix.
        /// </summary>
        /// <returns>The value of the determinant.</returns>
        public Complex Det()
        {
            if (rows == columns)
            {
                var n = columns;

                if (n == 1)
                    return this[0, 0];
                else if (n == 2)
                    return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
                else if (n == 3)
                {
                    return this[0, 0] * (this[1, 1] * this[2, 2] - this[1, 2] * this[2, 1]) +
                            this[0, 1] * (this[1, 2] * this[2, 0] - this[1, 0] * this[2, 2]) +
                            this[0, 2] * (this[1, 0] * this[2, 1] - this[1, 1] * this[2, 0]);
                }
                else if (n == 4)
                {
                    //I guess that's right
                    return this[0, 0] * (this[1, 1] *
                                (this[2, 2] * this[3, 3] - this[2, 3] * this[3, 2]) + this[1, 2] *
                                    (this[2, 3] * this[3, 1] - this[2, 1] * this[3, 3]) + this[1, 3] *
                                        (this[2, 1] * this[3, 2] - this[2, 2] * this[3, 1])) -
                            this[0, 1] * (this[1, 0] *
                                (this[2, 2] * this[3, 3] - this[2, 3] * this[3, 2]) + this[1, 2] *
                                    (this[2, 3] * this[3, 0] - this[2, 0] * this[3, 3]) + this[1, 3] *
                                        (this[2, 0] * this[3, 2] - this[2, 2] * this[3, 0])) +
                            this[0, 2] * (this[1, 0] *
                                (this[2, 1] * this[3, 3] - this[2, 3] * this[3, 1]) + this[1, 1] *
                                    (this[2, 3] * this[3, 0] - this[2, 0] * this[3, 3]) + this[1, 3] *
                                        (this[2, 0] * this[3, 1] - this[2, 1] * this[3, 0])) -
                            this[0, 3] * (this[1, 0] *
                                (this[2, 1] * this[3, 2] - this[2, 2] * this[3, 1]) + this[1, 1] *
                                    (this[2, 2] * this[3, 0] - this[2, 0] * this[3, 2]) + this[1, 2] *
                                        (this[2, 0] * this[3, 1] - this[2, 1] * this[3, 0]));
                }

                var lu = new LUDecomposition(this);
                return lu.Determinant();
            }

            return Complex.Zero;
        }

        #endregion

        #region Vectors

        /// <summary>
        /// Gets the j-th row vector, i.e. a vector which is spanned over all columns of one row.
        /// </summary>
        /// <param name="j">The index of the row to get the vector from.</param>
        /// <returns>The extracted row vector.</returns>
        public Matrix GetRowVector(int j)
        {
            var m = new Matrix(1, Columns);

            for (var i = 0; i < Columns; i++)
                m[0, i] = this[j, i];

            return m;
        }

        /// <summary>
        /// Sets the j-th row vector to be of the given matrix.
        /// </summary>
        /// <param name="j">The index of the row to set the vector to.</param>
        /// <param name="M">The matrix with values to set the j-th row to.</param>
        /// <returns>The current instance.</returns>
        public Matrix SetRowVector(int j, Matrix M)
        {
            for (var i = 0; i < M.Length; i++)
                this[j, i] = M[i];

            return this;
        }

        /// <summary>
        /// Gets the i-th column vector, i.e. a vector which is spanned over all rows of one column.
        /// </summary>
        /// <param name="i">The index of the column to get the vector from.</param>
        /// <returns>The extracted column vector.</returns>
        public Matrix GetColumnVector(int i)
        {
            var m = new Matrix(Rows, 1);

            for (var j = 0; j < Rows; j++)
                m[j, 0] = this[j, i];

            return m;
        }

        /// <summary>
        /// Sets the i-th column vector to be of the given matrix.
        /// </summary>
        /// <param name="i">The index of the column to set the vector to.</param>
        /// <param name="M">The matrix with values to set the i-th column to.</param>
        /// <returns>The current instance.</returns>
        public Matrix SetColumnVector(int i, Matrix M)
        {
            for (var j = 0; j < M.Length; j++)
                this[j, i] = M[j];

            return this;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the L2-Norm of the matrix (seen as a vector).
        /// </summary>
        /// <returns>The square root of the absolute squared values.</returns>
        public Double Abs()
        {
            var sum = 0.0;

            for (var j = 0; j < rows; j++)
            {
                for (var i = 0; i < columns; i++)
                {
                    var c = this[j, i];
                    sum += (c.Re * c.Re + c.Im * c.Im);
                }
            }

            return Math.Sqrt(sum);
        }

        /// <summary>
        /// Performs the operation f on each element of the matrix, creating a new matrix
        /// B where each entry is given by B_jk = f(A_jk).
        /// </summary>
        /// <param name="f">The function to use.</param>
        /// <returns>The created matrix.</returns>
        public Matrix ForEach(Func<Complex, Complex> f)
        {
            var M = new Matrix(rows, columns);

            for (var j = 0; j < rows; j++)
                for (var i = 0; i < columns; i++)
                    M[j, i] = f(this[j, i]);

            return M;
        }

        /// <summary>
        /// Goes over all rows and columns and randomizes the values.
        /// </summary>
        public void Randomize()
        {
            var r = new Random();

            for (var j = 0; j < rows; j++)
                for (var i = 0; i < columns; i++)
                    this[j, i] = new Complex(r.NextDouble());
        }

        #endregion

        #region Operators

        /// <summary>
        /// Multiplication.
        /// </summary>
        /// <param name="x">Matrix A</param>
        /// <param name="y">Matrix B</param>
        /// <returns>A * B</returns>
        public static Matrix operator *(Matrix x, Matrix y)
        {
            if (x.columns != y.rows)
                throw new YException("The columns ({0}) of the first matrix do not match the rows ({1}) of the second matrix.", x.columns, y.rows);

            var A = x.ToArray();
            var B = y.ToArray();
            var C = new Complex[x.rows * y.columns];
            BlasL3.cGemm(A, 0, x.columns, 1, B, 0, y.columns, 1, C, 0, y.columns, 1, x.rows, y.columns, x.columns);
            return new Matrix(C, x.rows, y.columns);
        }

        /// <summary>
        /// Multiplication.
        /// </summary>
        /// <param name="s">Scalar s</param>
        /// <param name="M">Matrix M</param>
        /// <returns>s * M</returns>
        public static Matrix operator *(Complex s, Matrix M)
        {
            var A = new Matrix(M.rows, M.columns);

            if (s == Complex.Zero)
                return A;

            for (var i = 0; i < M.columns; i++)
            {
                for (var j = 0; j < M.rows; j++)
                {
                    var m = M[j, i];

                    if (m != Complex.Zero)
                        A[j, i] = m * s;
                }
            }

            return A;
        }

        /// <summary>
        /// Multiplication.
        /// </summary>
        /// <param name="M">Matrix M</param>
        /// <param name="s">Scalar s</param>
        /// <returns>s * M</returns>
        public static Matrix operator *(Matrix M, Complex s)
        {
            var A = new Matrix(M.rows, M.columns);

            if (s == Complex.Zero)
                return A;

            for (var i = 0; i < M.columns; i++)
            {
                for (var j = 0; j < M.rows; j++)
                {
                    var m = M[j, i];

                    if (m != Complex.Zero)
                        A[j, i] = m * s;
                }
            }

            return A;
        }

        /// <summary>
        /// Division.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l / r</returns>
        public static Matrix operator /(Matrix l, Matrix r)
        {
            return l * r.Inv();
        }
        
        /// <summary>
        /// Division.
        /// </summary>
        /// <param name="l">Scalar l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l / r</returns>
        public static Matrix operator /(Complex l, Matrix r)
        {
            return l * r.Inv();
        }

        /// <summary>
        /// Division.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Scalar r</param>
        /// <returns>l / r</returns>
        public static Matrix operator /(Matrix l, Complex r)
        {
            var m = new Matrix(l.rows, l.columns);

            for (var j = 0; j < l.rows; j++)
                for (var i = 0; i < l.columns; i++)
                    m[j, i] = l[j, i] / r;

            return m;
        }

        /// <summary>
        /// Subtraction.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l - r</returns>
        public static Matrix operator -(Matrix l, Matrix r)
        {
            if (r.columns != l.columns || r.rows != l.rows)
                throw new YException("The matrix dimensions do not agree.");

            var A = l.ToArray();
            var B = r.ToArray();
            var n = l.Length;
            var C = new Complex[n];

            for (var k = 0; k != n; k++)
                C[k] = A[k] - B[k];

            return new Matrix(C, r.rows, r.columns);
        }

        /// <summary>
        /// Subtraction.
        /// </summary>
        /// <param name="l">Complex l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l + r</returns>
        public static Matrix operator -(Complex l, Matrix r)
        {
            var B = r.ToArray();
            var n = r.Length;
            var C = new Complex[n];

            for (var k = 0; k != n; k++)
                C[k] = l - B[k];

            return new Matrix(C, r.rows, r.columns);
        }

        /// <summary>
        /// Subtraction.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Complex r</param>
        /// <returns>l + r</returns>
        public static Matrix operator -(Matrix l, Complex r)
        {
            var A = l.ToArray();
            var n = l.Length;
            var C = new Complex[n];

            for (var k = 0; k != n; k++)
                C[k] = A[k] - r;

            return new Matrix(C, l.rows, l.columns);
        }

        /// <summary>
        /// Addition.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l + r</returns>
        public static Matrix operator +(Matrix l, Matrix r)
        {
            if (r.columns != l.columns || r.rows != l.rows)
                throw new YException("The matrix dimensions do not agree.");

            var A = l.ToArray();
            var B = r.ToArray();
            var n = l.Length;
            var C = new Complex[n];

            for (var k = 0; k != n; k++)
                C[k] = A[k] + B[k];

            return new Matrix(C, r.rows, r.columns);
        }

        /// <summary>
        /// Addition.
        /// </summary>
        /// <param name="l">Complex l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l + r</returns>
        public static Matrix operator +(Complex l, Matrix r)
        {
            var B = r.ToArray();
            var n = r.Length;
            var C = new Complex[n];

            for (var k = 0; k != n; k++)
                C[k] = l + B[k];

            return new Matrix(C, r.rows, r.columns);
        }

        /// <summary>
        /// Addition.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Complex r</param>
        /// <returns>l + r</returns>
        public static Matrix operator +(Matrix l, Complex r)
        {
            var A = l.ToArray();
            var n = l.Length;
            var C = new Complex[n];

            for (var k = 0; k != n; k++)
                C[k] = A[k] + r;

            return new Matrix(C, l.rows, l.columns);
        }

        /// <summary>
        /// Equality.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l == r</returns>
        public static Matrix operator ==(Matrix l, Matrix r)
        {
            if (ReferenceEquals(l, r))
                return Ones(l.rows, l.columns);

            var cols = Math.Min(l.columns, r.columns);
            var rows = Math.Min(l.rows, r.rows);
            var M = Zeros(Math.Max(l.rows, r.rows), Math.Max(l.columns, r.columns));

            for (var i = 0; i < cols; i++)
            {
                for (var j = 0; j < rows; j++)
                    M[j, i] = l[j, i] == r[j, i] ? 1 : 0;
            }

            return M;
        }

        /// <summary>
        /// Inequality.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l != r</returns>
        public static Matrix operator !=(Matrix l, Matrix r)
        {
            if (ReferenceEquals(l, r))
                return Zeros(l.rows, l.columns);

            var cols = Math.Min(l.columns, r.columns);
            var rows = Math.Min(l.rows, r.rows);
            var M = Ones(Math.Max(l.rows, r.rows), Math.Max(l.columns, r.columns));

            for (var i = 0; i < cols; i++)
            {
                for (var j = 0; j < rows; j++)
                    M[j, i] = l[j, i] == r[j, i] ? 0 : 1;
            }

            return M;
        }

        /// <summary>
        /// Greater.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l > r</returns>
        public static Matrix operator >(Matrix l, Matrix r)
        {
            if (ReferenceEquals(l, r))
                return Zeros(l.rows, l.columns);

            var cols = Math.Min(l.columns, r.columns);
            var rows = Math.Min(l.rows, r.rows);
            var M = Zeros(Math.Max(l.rows, r.rows), Math.Max(l.columns, r.columns));

            for (var i = 0; i < cols; i++)
            {
                for (var j = 0; j < rows; j++)
                    M[j, i] = l[j, i] > r[j, i] ? 1 : 0;
            }

            return M;
        }

        /// <summary>
        /// Smaller.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l &lt; r</returns>
        public static Matrix operator <(Matrix l, Matrix r)
        {
            if (ReferenceEquals(l, r))
                return Zeros(l.rows, l.columns);

            var cols = Math.Min(l.columns, r.columns);
            var rows = Math.Min(l.rows, r.rows);
            var M = Zeros(Math.Max(l.rows, r.rows), Math.Max(l.columns, r.columns));

            for (var i = 0; i < cols; i++)
            {
                for (var j = 0; j < rows; j++)
                    M[j, i] = l[j, i] < r[j, i] ? 1 : 0;
            }

            return M;
        }

        /// <summary>
        /// Greater or equal.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l >= r</returns>
        public static Matrix operator >=(Matrix l, Matrix r)
        {
            if (ReferenceEquals(l, r))
                return Ones(l.rows, l.columns);

            var cols = Math.Min(l.columns, r.columns);
            var rows = Math.Min(l.rows, r.rows);
            var M = Zeros(Math.Max(l.rows, r.rows), Math.Max(l.columns, r.columns));

            for (var i = 0; i < cols; i++)
            {
                for (var j = 0; j < rows; j++)
                    M[j, i] = l[j, i] >= r[j, i] ? 1 : 0;
            }

            return M;
        }

        /// <summary>
        /// Smaller or equal.
        /// </summary>
        /// <param name="l">Matrix l</param>
        /// <param name="r">Matrix r</param>
        /// <returns>l &lt;= r</returns>
        public static Matrix operator <=(Matrix l, Matrix r)
        {
            if (ReferenceEquals(l, r))
                return Ones(l.rows, l.columns);

            var cols = Math.Min(l.columns, r.columns);
            var rows = Math.Min(l.rows, r.rows);
            var M = Zeros(Math.Max(l.rows, r.rows), Math.Max(l.columns, r.columns));

            for (var i = 0; i < cols; i++)
            {
                for (var j = 0; j < rows; j++)
                    M[j, i] = l[j, i] <= r[j, i] ? 1 : 0;
            }

            return M;
        }

        /// <summary>
        /// Unary minus.
        /// </summary>
        /// <param name="m">Matrix m</param>
        /// <returns>l - r</returns>
        public static Matrix operator -(Matrix m)
        {
            return m.ForEach(z => -z);
        }

        #endregion

        #region Equality

        /// <summary>
        /// Gets the hashcode of the matrix.
        /// </summary>
        /// <returns>The length of the matrix.</returns>
        public override int GetHashCode()
        {
            return Length;
        }

        /// <summary>
        /// Checks if the given object is equal to this object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the other object is also a matrix, which is the same or has the same content, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;

            if (obj is Matrix)
            {
                var m = (Matrix)obj;

                if (columns == m.columns && rows == m.rows)
                {
                    for (int i = 0; i < Length; i++)
                    {
                        if (this[i] != m[i])
                            return false;
                    }

                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Submatrices

        /// <summary>
        /// Deletes the specified number of columns of the matrix.
        /// </summary>
        /// <param name="index">The column index to start (0-based).</param>
        /// <param name="count">The number of columns to remove.</param>
        public void DeleteColumns(int index, int count = 1)
        {
            if (count < 0)
            {
                index += count;
                count = -count;
            }
            else if (count == 0)
                return;

            if (index + count > columns)
                count = columns - index;

            var dim = index + count;

            for (var i = index; i < dim; i++)
            {
                for (var j = 0; j < rows; j++)
                    this[j, i] = 0.0;
            }

            index = dim;

            for (var i = index; i < columns; i++)
            {
                var k = i - count;

                for (var j = 0; j < rows; j++)
                {
                    this[j, k] = this[j, i];
                    this[j, i] = 0.0;
                }
            }

            columns -= count;
        }

        /// <summary>
        /// Deletes the specified number of rows of the matrix.
        /// </summary>
        /// <param name="index">The row index to start (0-based).</param>
        /// <param name="count">The number of rows to remove.</param>
        public void DeleteRows(int index, int count = 1)
        {
            if (count < 0)
            {
                index += count;
                count = -count;
            }
            else if (count == 0)
                return;

            if (index + count > rows)
                count = rows - index;

            var dim = index + count;

            for (var j = index; j < dim; j++)
            {
                for (var i = 0; i < columns; i++)
                    this[j, i] = 0.0;
            }

            index = dim;

            for (var j = index; j < rows; j++)
            {
                var k = j - count;

                for (var i = 0; i < columns; i++)
                {
                    this[k, i] = this[j, i];
                    this[j, i] = 0.0;
                }
            }

            rows -= count;
        }

        /// <summary>
        /// Gets a real matrix of the complete matrix.
        /// </summary>
        /// <returns>A jagged 2D array.</returns>
        public double[][] GetRealMatrix()
        {
            var array = new double[rows][];

            for (var j = 0; j < rows; j++)
            {
                array[j] = new double[columns];

                for (var i = 0; i < columns; i++)
                    array[j][i] = this[j, i].Re;
            }

            return array;
        }

        /// <summary>
        /// Gets a real matrix of the complete matrix.
        /// </summary>
        /// <returns>A jagged 2D array.</returns>
        public Complex[][] GetComplexMatrix()
        {
            var array = new Complex[rows][];

            for (var j = 0; j < rows; j++)
            {
                array[j] = new Complex[columns];

                for (var i = 0; i < columns; i++)
                    array[j][i] = this[j, i];
            }

            return array;
        }

        /// <summary>
        /// Creates a sub matrix of the given instance.
        /// </summary>
        /// <param name="yoffset">Vertical offset in rows.</param>
        /// <param name="yfinal">Final row-index.</param>
        /// <param name="xoffset">Horizontal offset in columns.</param>
        /// <param name="xfinal">Final column-index.</param>
        /// <returns>The new instance with the corresponding entries.</returns>
        public Matrix GetSubMatrix(int yoffset, int yfinal, int xoffset, int xfinal)
        {
            var X = new Matrix(yfinal - yoffset, xfinal - xoffset);

            for (int j = yoffset; j < yfinal; j++)
                for (int i = xoffset; i < xfinal; i++)
                    X[j - yoffset, i - xoffset] = this[j, i];

            return X;
        }

        /// <summary>
        /// Creates a sub matrix of the given instance.
        /// </summary>
        /// <param name="y">Row-indices to consider.</param>
        /// <param name="xoffset">Horizontal offset in columns.</param>
        /// <param name="xfinal">Final column-index.</param>
        /// <returns>The new instance with the corresponding entries.</returns>
        public Matrix GetSubMatrix(int[] y, int xoffset, int xfinal)
        {
            var X = new Matrix(y.Length, xfinal - xoffset);

            for (int j = 0; j < y.Length; j++)
                for (int i = xoffset; i < xfinal; i++)
                    X[j, i - xoffset] = this[y[j], i];

            return X;
        }

        #endregion

        #region String representation

        public override String ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    sb.Append(this[i, j].ToString());

                    if (j + 1 < columns)
                        sb.Append(", ");
                }

                if (i + 1 < rows)
                    sb.Append("; ");
            }

            return sb.ToString();
        }

        public String ToCode()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                sb.Append("( ");

                for (int j = 0; j < columns; j++)
                {
                    sb.Append(this[i, j].ToString().PadRight(10, ' '));
                }

                sb.Append(" )");

                if (i + 1 < rows)
                    sb.AppendLine();
            }

            return sb.ToString();
        }

        public String ToString(String format, IFormatProvider formatProvider)
        {
            //TODO
            return ToString();
        }

        #endregion

        #region Interface implementation

        public int CompareTo(object obj)
        {
            if (obj is Matrix)
                return CompareTo((Matrix)obj);

            return -1;
        }

        public int CompareTo(Matrix other)
        {
            if(this.Equals(other))
                return 0;

            var myval = Det();
            var urval = other.Det();

            if (myval > urval)
                return 1;
            else if (myval < urval)
                return -1;

            return 0;
        }

        public bool Equals(Matrix other)
        {
            if (this.Equals((object)other))
                return true;

            return false;
        }

        #endregion

        #region Static Math functions

        /// <summary>
        /// Computes s^M, i.e. a (in general) complex number to the power of a matrix.
        /// </summary>
        /// <param name="s">The complex number.</param>
        /// <param name="M">The matrix.</param>
        /// <returns>A matrix which has the same dimension of the passed matrix.</returns>
        public static Matrix Pow(Complex s, Matrix M)
        {
            return M.ForEach(z => s.Pow(z));
        }

        /// <summary>
        /// Computes M^s, where s has to be an integer.
        /// </summary>
        /// <param name="M">The matrix M.</param>
        /// <param name="s">The integer s.</param>
        /// <returns>The matrix M, s times.</returns>
        public static Matrix Pow(Matrix M, Complex s)
        {
            if (M.columns != M.rows)
                throw new YException("The matrix has to be a square matrix.");
            else if (s.Im != 0.0)
                throw new YException("Complex numbers are not allowed.");
            else if (Math.Floor(s.Re) != s.Re)
                throw new YException("Matrices can be powered only with integers.");

            var A = Matrix.One(M.columns);
            var B = s < 0 ? M.Inv() : M;
            var count = (Int32)Math.Abs(s.Re);

            for (var i = 0; i < count; i++)
                A = A * B;

            return A;
        }

        #endregion

        #region Helpers

        void LogicalSubscripting(Matrix m, List<Index> indices)
        {
            for (var i = 0; i < m.columns; i++)
            {
                for (var j = 0; j < m.rows; j++)
                {
                    if (m[j, i].Re != 0.0)
                    {
                        indices.Add(new Index
                        {
                            j = j,
                            i = i
                        });
                    }
                }
            }
        }

        void DeleteFullRows(List<Index> indices)
        {
            // There cannot be a full row if we have less indices than columns
            if (indices.Count < this.columns)
                return;

            // Save currently found rows
            var rows = new List<int>();

            // Go over the all the indices
            for (var i = 0; i < indices.Count; i++)
            {
                var row = indices[i].j;
                var columns = new List<int>();
                var taken = new List<int>();
                columns.Add(indices[i].i);
                taken.Add(i);

                // How often do we find the same row ?
                for (var j = i + 1; j < indices.Count; j++)
                {
                    if (indices[j].j == row && !columns.Contains(indices[j].i) && indices[j].i >= 1 && indices[j].i <= Columns)
                    {
                        columns.Add(indices[j].i);
                        taken.Add(j);

                        // Apparently we've found a complete row - stop.
                        if (columns.Count == this.columns)
                            break;
                    }
                }

                // Check again if we found a match
                if (columns.Count == this.columns)
                {
                    // Sort the found indices
                    taken.Sort();

                    // Remove the indices to improve search performance
                    for (var k = taken.Count - 1; k >= 0; k--)
                        indices.RemoveAt(taken[k]);

                    // Add the found row to the list of found rows
                    rows.Add(row);
                    // Start again from the beginning, since we removed indices
                    i = -1;
                }
            }

            // Just sort the rows to optimize the next itertion
            rows.Sort();

            // Go over all found rows
            for (var i = rows.Count - 1; i >= 0; i--)
            {
                var count = 1;

                // Look how many consecutive rows we found
                for (var j = i - 1; j >= 0; j--)
                {
                    if (rows[j] == rows[j + 1] - 1)
                        count++;
                    else
                        break;
                }

                // Modify the loop iterator (for count == 1 no change is required)
                i -= (count - 1);
                // Delete the consecutive rows
                DeleteRows(rows[i], count);
                // Modify the other indices - shift them by the number of rows if required
                var minIndex = rows[i] + count;

                for (var j = 0; j < indices.Count; j++)
                {
                    if (indices[j].j >= minIndex)
                        indices[j] = new Index
                        {
                            j = indices[j].j - count,
                            i = indices[j].i
                        };
                }
            }
        }

        void DeleteFullColumns(List<Index> indices)
        {
            // There cannot be a full column if we have less indices than rows
            if (indices.Count < this.rows)
                return;

            // Save currently found columns
            var columns = new List<int>();

            // Go over the all the indices
            for (var i = 0; i < indices.Count; i++)
            {
                var col = indices[i].i;
                var rows = new List<int>();
                var taken = new List<int>();
                rows.Add(indices[i].j);
                taken.Add(i);

                // How often do we find the same column ?
                for (var j = i + 1; j < indices.Count; j++)
                {
                    if (indices[j].i == col && !rows.Contains(indices[j].j) && indices[j].j >= 1 && indices[j].j <= Rows)
                    {
                        rows.Add(indices[j].j);
                        taken.Add(j);

                        // Apparently we've found a complete column - stop.
                        if (rows.Count == this.rows)
                            break;
                    }
                }

                if (rows.Count == this.rows)
                {
                    // Sort the found indices
                    taken.Sort();

                    // Remove the indices to improve search performance
                    for (var k = taken.Count - 1; k >= 0; k--)
                        indices.RemoveAt(taken[k]);

                    // Add the found column to the list of found columns
                    columns.Add(col);
                    // Start again from the beginning, since we removed indices
                    i = -1;
                }
            }

            // Just sort the columns to optimize the next itertion
            columns.Sort();

            // Go over all found columns
            for (var i = columns.Count - 1; i >= 0; i--)
            {
                var count = 1;

                // Look how many consecutive columns we found
                for (var j = i - 1; j >= 0; j--)
                {
                    if (columns[j] == columns[j + 1] - 1)
                        count++;
                    else
                        break;
                }

                // Modify the loop iterator (for count == 1 no change is required)
                i -= count - 1;
                // Delete the consecutive columns
                DeleteColumns(columns[i], count);
                // Modify the other indices - shift them by the number of columns if required
                var minIndex = columns[i] + count;

                for (var j = 0; j < indices.Count; j++)
                {
                    if (indices[j].i >= minIndex)
                        indices[j] = new Index
                        {
                            j = indices[j].j, 
                            i = indices[j].i - count
                        };
                }
            }
        }

        #endregion

        #region Functional behavior

        internal void Set(object[] obj, object values)
        {
            var indices = new List<Index>();

            if (obj.Length == 1)
            {
                // A matrix as argument == probably logical subscripting?
                if (obj[0] is Matrix)
                {
                    var mm = (Matrix)obj[0];

                    // But only logical subscripting of the dimensions match!
                    if (mm.columns == columns && mm.rows == rows)
                        LogicalSubscripting(mm, indices);
                    else if (mm.IsVector)
                    {
                        var length = Math.Max(1, rows);

                        for (int i = 0; i < mm.Length; i++)
                        {
                            var k = (int)mm[i].Re;
                            var row = k % length + 1;
                            var col = k / length + 1;

                            indices.Add(new Index
                            {
                                j = row,
                                i = col
                            });
                        }
                    }
                    else
                        throw new YException("The matrix dimensions do not agree.");
                }
                else
                {
                    var length = Math.Max(1, rows);
                    var k = (int)obj[0];
                    var row = (k - 1) % length + 1;
                    var col = (k - 1) / length + 1;

                    indices.Add(new Index
                    {
                        j = row,
                        i = col
                    });
                }
            }
            else
            {
                var rows = obj[0] is Int64 ? new Int64[] { (Int64)obj[0] } : (Int64[])obj[0];
                var columns = obj[1] is Int64 ? new Int64[] { (Int64)obj[1] } : (Int64[])obj[1];

                for (int i = 0; i < columns.Length; i++)
                {
                    for (int j = 0; j < rows.Length; j++)
                    {
                        indices.Add(new Index
                        {
                            i = (int)columns[i],
                            j = (int)rows[j]
                        });
                    }
                }
            }

            // The right hand side is a matrix (look for dimensions)
            if (values is Matrix)
            {
                var index = 0;
                var m = (Matrix)values;

                // Special case: Empty matrix supplied
                if (m.rows == 0 && m.columns == 0)
                {
                    //Passed in empty matrix as in M(3,:) = []; --> either delete COMPLETE
                    //row or column, or set the corresponding values to 0 (if no complete
                    //row or column has been selected).

                    DeleteFullRows(indices);
                    DeleteFullColumns(indices);

                    if (indices.Count > 0)
                    {
                        for (var i = 0; i < indices.Count; i++)
                            this[indices[i].j, indices[i].i] = 0.0;
                    }
                }
                // In the general case the right hand side matrix has to be of the same 
                // dimension (or length) as the supplied indices.
                else
                {
                    if (m.Length != indices.Count)
                        throw new YException("The matrix dimensions do not agree.");

                    foreach (var mi in indices)
                        this[mi.j, mi.i] = m[index++];
                }
            }
            // the right hand side is a scalar (apply to each value)
            else
            {
                var value = (Complex)values;

                foreach (var mi in indices)
                    this[mi.j, mi.i] = value;
            }
        }

        #endregion

        #region IList Implementation

        int IList.Add(object value)
        {
            ((IList)this)[Length] = value;
            return Length;
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.Contains(object value)
        {
            return false;
        }

        int IList.IndexOf(object value)
        {
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            ((IList)this)[index] = value;
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
        }

        void IList.RemoveAt(int index)
        {
            this[index] = 0;
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                //if(YAMP.Types.ComplexType.Instance.IsDirect(value))
                //    this[index] = (Complex)value;
                //else if (YAMP.Types.ComplexType.Instance.IsIndirect(value))
                //    this[index] = (Complex)YAMP.Types.ComplexType.Instance.Cast(value);
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            for (int i = 0; i < Length; i++)
                array.SetValue(this[i], index + i);   
        }

        int ICollection.Count
        {
            get { return Length; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return this[i];
        }

        #endregion
    }
}
