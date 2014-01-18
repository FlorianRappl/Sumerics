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
using System.Collections.Generic;

namespace YAMP.Core
{
    /// <summary>
    /// Every function needs to implement the IFunction interface.
    /// </summary>
	public interface IFunction
    {
        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the minimum number of parameters to call the function.
        /// </summary>
        Int32 MinParameters { get; }

        /// <summary>
        /// Gets the maximum number of parameters to call the function.
        /// </summary>
        Int32 MaxParameters { get; }

        /// <summary>
        /// Gets a description of the function.
        /// </summary>
        String Description { get; }

        /// <summary>
        /// Gets a hyperlink with more information.
        /// </summary>
        String HyperReference { get; }

        /// <summary>
        /// Gets the category of the function.
        /// </summary>
        String Category { get; }

        /// <summary>
        /// Gets or sets a function resolver.
        /// </summary>
        IFunctionResolver Resolver { get; set; }

        /// <summary>
        /// Gets all the possible overloads (instances) for this function.
        /// </summary>
        IEnumerable<IFunctionItem> Overloads { get; }
	}
}