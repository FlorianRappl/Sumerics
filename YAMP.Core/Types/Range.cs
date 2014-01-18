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

namespace YAMP
{
    /// <summary>
    /// An object for generating vectors and more.
    /// </summary>
    [Serializable]
    public class Range<T> : IEnumerable<T>
    {
        #region Members

        readonly T start;
        readonly T end;
        readonly T step;
        readonly int count;

        Func<T, T, T> add;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new Range object with the given parameters.
        /// </summary>
        /// <param name="start">The start point of the range.</param>
        /// <param name="step">The step between two points.</param>
        /// <param name="end">The end point of the range.</param>
        /// <param name="count">The number of elements within the range.</param>
        /// <param name="add">The function to add the elements.</param>
        internal Range(T start, T step, T end, int count, Func<T, T, T> add)
        {
            this.start = start;
            this.step = step;
            this.end = end;
            this.count = count;
            this.add = add;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of elements in the vector.
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// Gets the (inclusive) start value.
        /// </summary>
        public T Start
        {
            get { return start; }
        }

        /// <summary>
        /// Gets the gap between two iterations.
        /// </summary>
        public T Step
        {
            get { return step; }
        }

        /// <summary>
        /// Gets the (inclusive) end point value.
        /// </summary>
        public T End
        {
            get { return end; }
        }

        #endregion

        #region IEnumerable implementation

        /// <summary>
        /// Gets an enumerator over all values.
        /// </summary>
        /// <returns>The specialized IEnumerator instance.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var current = start;
            var i = 0;
            var n = count;

            while (i++ < n)
            {
                yield return current;
                current = add(current, step);
            }
        }

        /// <summary>
        /// Gets an enumerator over all values.
        /// </summary>
        /// <returns>The general IEnumerator instance.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transforms the range to a linear array of real values
        /// with a modification applied to each entry.
        /// </summary>
        /// <param name="mod">The modification for each value.</param>
        /// <returns>The array with values.</returns>
        public T[] ForEach(Func<T, T> mod)
        {
            var A = new T[Count];
            var i = 0;
            var it = GetEnumerator();

            while (it.MoveNext())
                A[i++] = mod(it.Current);

            return A;
        }

        /// <summary>
        /// Transforms the range to a linear array of real values.
        /// </summary>
        /// <returns>The array with values.</returns>
        public T[] ToArray()
        {
            var A = new T[Count];
            var i = 0;
            var it = GetEnumerator();

            while (it.MoveNext())
                A[i++] = it.Current;

            return A;
        }

        #endregion
    }

    #region Creators

    /// <summary>
    /// A list of methods of possible range creators.
    /// </summary>
    public static class Range
    {
        /// <summary>
        /// Creates an integer range from the given start to the end.
        /// </summary>
        /// <param name="start">The inclusive start of the range.</param>
        /// <param name="end">The inclusive end of the range.</param>
        /// <returns>The created integer range.</returns>
        public static Range<Int64> Create(Int64 start, Int64 end)
        {
            var step = Math.Sign(end - start);
            var count = (int)((end - start) / step) + 1;
            return new Range<Int64>(start, step, end, count, (a, b) => a + b);
        }

        /// <summary>
        /// Creates an integer range from the given start to the end with the given step size.
        /// </summary>
        /// <param name="start">The inclusive start of the range.</param>
        /// <param name="end">The inclusive end of the range.</param>
        /// <param name="step">The step length between two numbers within the range.</param>
        /// <returns>The created integer range.</returns>
        public static Range<Int64> Create(Int64 start, Int64 end, Int64 step)
        {
            var count = (int)((end - start) / step) + 1;
            return new Range<Int64>(start, step, end, count, (a, b) => a + b);
        }

        /// <summary>
        /// Creates a real range from the given start to the end.
        /// </summary>
        /// <param name="start">The inclusive start of the range.</param>
        /// <param name="end">The inclusive end of the range.</param>
        /// <returns>The created real range.</returns>
        public static Range<Double> Create(Double start, Double end)
        {
            var step = Math.Sign(end - start);
            var count = (int)((end - start) / step) + 1;
            return new Range<Double>(start, step, end, count, (a, b) => a + b);
        }

        /// <summary>
        /// Creates a real range from the given start to the end with the given step size.
        /// </summary>
        /// <param name="start">The inclusive start of the range.</param>
        /// <param name="end">The inclusive end of the range.</param>
        /// <param name="step">The step length between two numbers within the range.</param>
        /// <returns>The created real range.</returns>
        public static Range<Double> Create(Double start, Double end, Double step)
        {
            var count = (int)((end - start) / step) + 1;
            return new Range<Double>(start, step, end, count, (a, b) => a + b);
        }
    }

    #endregion
}
