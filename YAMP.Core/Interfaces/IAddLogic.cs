/*
	Copyright (c) 2012-2013, Florian Rappl.
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

namespace YAMP.Core
{
    /// <summary>
    /// The interface for a class to register certain (binary) logic operators.
    /// </summary>
    public interface IAddLogic
    {
        /// <summary>
        /// Performs a greater than operation.
        /// </summary>
        /// <param name="method">The method that maps two incoming objects to a result.</param>
        /// <param name="operandType">The type of the left and right operands.</param>
        void Greater(Func<object, object, object> method, IType operandType);

        /// <summary>
        /// Performs a greater or equal to operation.
        /// </summary>
        /// <param name="method">The method that maps two incoming objects to a result.</param>
        /// <param name="operandType">The type of the left and right operands.</param>
        void GreaterEqual(Func<object, object, object> method, IType operandType);

        /// <summary>
        /// Performs a less than operation.
        /// </summary>
        /// <param name="method">The method that maps two incoming objects to a result.</param>
        /// <param name="operandType">The type of the left and right operands.</param>
        void Less(Func<object, object, object> method, IType operandType);

        /// <summary>
        /// Performs a less or equal to operation.
        /// </summary>
        /// <param name="method">The method that maps two incoming objects to a result.</param>
        /// <param name="operandType">The type of the left and right operands.</param>
        void LessEqual(Func<object, object, object> method, IType operandType);

        /// <summary>
        /// Performs a equal to operation.
        /// </summary>
        /// <param name="method">The method that maps two incoming objects to a result.</param>
        /// <param name="operandType">The type of the left and right operands.</param>
        void Equal(Func<object, object, object> method, IType operandType);

        /// <summary>
        /// Performs a not equal to operation.
        /// </summary>
        /// <param name="method">The method that maps two incoming objects to a result.</param>
        /// <param name="operandType">The type of the left and right operands.</param>
        void NotEqual(Func<object, object, object> method, IType operandType);
    }
}
