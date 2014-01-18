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
using System.Runtime.InteropServices;

namespace YAMP.Core
{
    /// <summary>
    /// Represents a color.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Color
    {
        #region Members

        [FieldOffset(0)]
        byte _red;
        [FieldOffset(1)]
        byte _green;
        [FieldOffset(2)]
        byte _blue;
        [FieldOffset(3)]
        byte _alpha;
        [FieldOffset(0)]
        int _hashcode;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new color with the specified primitives.
        /// </summary>
        /// <param name="red">The red part.</param>
        /// <param name="green">The green part.</param>
        /// <param name="blue">The blue part.</param>
        public Color(byte red, byte green, byte blue)
        {
            _hashcode = 0;
            _red = red;
            _green = green;
            _blue = blue;
            _alpha = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the red color value.
        /// </summary>
        public byte R
        {
            get { return _red; }
        }

        /// <summary>
        /// Gets the green color value.
        /// </summary>
        public byte G
        {
            get { return _green; }
        }

        /// <summary>
        /// Gets the blue color value.
        /// </summary>
        public byte B
        {
            get { return _blue; }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Transforms the color to a hex-string.
        /// </summary>
        /// <returns>The hex string.</returns>
        public override string ToString()
        {
            return "#" + _red.ToString("X2") + _green.ToString("X2") + _blue.ToString("X2");
        }

        #endregion

        #region Equality

        /// <summary>
        /// Checks if the given object is equal to this.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>The result of the comparison.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Color)
                return ((Color)obj)._hashcode == _hashcode;

            return false;
        }

        /// <summary>
        /// Gets the hashcode of the color.
        /// </summary>
        /// <returns>The 32-bit value.</returns>
        public override int GetHashCode()
        {
            return _hashcode;
        }

        /// <summary>
        /// Checks the two colors for equality.
        /// </summary>
        /// <param name="a">The first color.</param>
        /// <param name="b">The second color.</param>
        /// <returns>The result of the comparison.</returns>
        public static bool operator ==(Color a, Color b)
        {
            return a._hashcode == b._hashcode;
        }

        /// <summary>
        /// Checks the two colors for inequality.
        /// </summary>
        /// <param name="a">The first color.</param>
        /// <param name="b">The second color.</param>
        /// <returns>The result of the comparison.</returns>
        public static bool operator !=(Color a, Color b)
        {
            return a._hashcode != b._hashcode;
        }

        #endregion

        #region Standard colors

        /// <summary>
        /// Gets black (0, 0, 0).
        /// </summary>
        public static readonly Color Black = new Color(0, 0, 0);

        /// <summary>
        /// Gets yellow (255, 255, 0).
        /// </summary>
        public static readonly Color Yellow = new Color(255, 255, 0);

        /// <summary>
        /// Gets magenta (255, 0, 255).
        /// </summary>
        public static readonly Color Magenta = new Color(255, 0, 255);

        /// <summary>
        /// Gets white (255, 255, 255).
        /// </summary>
        public static readonly Color White = new Color(255, 255, 255);

        /// <summary>
        /// Gets red (255, 0, 0).
        /// </summary>
        public static readonly Color Red = new Color(255, 0, 0);

        /// <summary>
        /// Gets green (0, 255, 0).
        /// </summary>
        public static readonly Color Green = new Color(0, 255, 0);

        /// <summary>
        /// Gets blue (0, 0, 255).
        /// </summary>
        public static readonly Color Blue = new Color(0, 0, 255);

        #endregion
    }
}
