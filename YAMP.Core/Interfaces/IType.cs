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

namespace YAMP.Core
{
    /// <summary>
    /// Every type descriptor has to implement this interface.
    /// </summary>
    interface IType
    {
        /// <summary>
        /// Determines what the relation of the type to the given type is.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <returns>The relation between the types from the current type.</returns>
        TypeMetric RelationTo(IType type);

        /// <summary>
        /// Checks if the given object (which is directly of the described type) does
        /// convert to true.
        /// </summary>
        /// <param name="o">The object to examine.</param>
        /// <returns>True if it represents true, otherwise false.</returns>
        Boolean IsTrue(Object obj);

        /// <summary>
        /// Convert the (direct or indirect) object to the described type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The object in the described format.</returns>
        Object Convert(Dynamic value);

        /// <summary>
        /// Gets the name of the described type.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Checks if the given value is of the described type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True if it is of this type, otherwise false.</returns>
        Boolean IsType(Object value);

        /// <summary>
        /// Checks if the given two instances are equivalent.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object.</param>
        /// <returns>The result of the comparison.</returns>
        Boolean AreEqual(Object left, Object right);

        /// <summary>
        /// Checks if the given two instances are not equivalent.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object.</param>
        /// <returns>The result of the comparison.</returns>
        Boolean AreNotEqual(Object left, Object right);

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <param name="value">The value to represent.</param>
        /// <returns>The pure representation.</returns>
        String ToString(Object value);

        /// <summary>
        /// Returns the string representation in form of code of the object.
        /// </summary>
        /// <param name="value">The value to represent.</param>
        /// <returns>The code representation.</returns>
        String ToCode(Object value);
    }
}
