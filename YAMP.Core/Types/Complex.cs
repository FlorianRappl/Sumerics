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
using System.Globalization;
using YAMP.Numerics;

namespace YAMP
{
    /// <summary>
    /// Represents a complex number a + ib, where a is the real part and
    /// b is the imaginary part.
    /// </summary>
    [Serializable]
    public struct Complex : IComparable, IFormattable, IConvertible, IComparable<Complex>, IEquatable<Complex>
    {
        #region Members

        readonly Double re;
        readonly Double im;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new complex number.
        /// </summary>
        /// <param name="real">The real part of the number.</param>
        public Complex(Double real)
        {
            re = real;
            im = 0.0;
        }

        /// <summary>
        /// Creates a new complex number.
        /// </summary>
        /// <param name="real">The real part of the number.</param>
        /// <param name="imaginary">The imaginary part of the number.</param>
        public Complex(Double real, Double imaginary)
        {
            re = real;
            im = imaginary;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the zero value (0, 0).
        /// </summary>
        public static Complex Zero
        {
            get { return new Complex(0, 0); }
        }

        /// <summary>
        /// Gets the unit value (1, 0).
        /// </summary>
        public static Complex One
        {
            get { return new Complex(1, 0); }
        }

        /// <summary>
        /// Gets the imaginary unit (0, 1).
        /// </summary>
        public static Complex I
        {
            get { return new Complex(0, 1); }
        }

        /// <summary>
        /// Gets the real part of the complex number.
        /// </summary>
        public double Re
        {
            get { return re; }
        }

        /// <summary>
        /// Gets the imaginary part of the complex number.
        /// </summary>
        public double Im
        {
            get { return im; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the absolute value of the complex number.
        /// </summary>
        /// <returns>The absolute value.</returns>
        public double Abs()
        {
            return Math.Sqrt(re * re + im * im);
        }

        /// <summary>
        /// Computes the angle in the complex (gaussian) plane.
        /// </summary>
        /// <returns>The angle of the y-x ratio.</returns>
        public double Arg()
        {
            return Math.Atan2(im, re);
        }

        /// <summary>
        /// Conjugates the complex number.
        /// </summary>
        /// <returns>The conjugated number.</returns>
        public Complex Conj()
        {
            return new Complex(re, -im);
        }

        /// <summary>
        /// Raises the complex number to a certain power.
        /// </summary>
        /// <param name="y">The exponent.</param>
        /// <returns>The raised number.</returns>
        public Complex Pow(Complex y)
        {
            if (re == 0.0 && im == 0.0)
                return new Complex();

            var theta = re == 0.0 ? Math.PI / 2 * Math.Sign(im) : Math.Atan2(im, re);
            var li = re / Math.Cos(theta);
            var phi = Complex.Ln(this) * y.im;
            var ri = Complex.Exp(I * phi);
            var alpha = re == 0.0 ? 1.0 : Math.Pow(Math.Abs(li), y.re);
            var beta = theta * y.re;
            var cos = Math.Cos(beta);
            var sin = Math.Sin(beta);
            var a = alpha * (cos * ri.re - sin * ri.im);
            var b = alpha * (cos * ri.im + sin * ri.re);

            if (li < 0)
                return new Complex(-b, a);

            return new Complex(a, b);
        }

        #endregion

        #region General Math

        /// <summary>
        /// Raises the complex number to a certain power.
        /// </summary>
        /// <param name="x">The complex number.</param>
        /// <param name="y">The exponent.</param>
        /// <returns>The raised number.</returns>
        public static Complex Pow(Complex x, Complex y)
        {
            return x.Pow(y);
        }

        /// <summary>
        /// Takes the exponential of the complex number.
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The exponential value.</returns>
        public static Complex Exp(Complex c)
        {
            var f = Math.Exp(c.re);
            var re = f * Math.Cos(c.im);
            var im = f * Math.Sin(c.im);
            return new Complex(re, im);
        }

        /// <summary>
        /// Takes the natural logarithm of the complex number.
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The natural logarithm.</returns>
        public static Complex Ln(Complex c)
        {
            var re = Math.Log(Math.Sqrt(c.re * c.re + c.im * c.im));
            var im = c.Arg();
            return new Complex(re, im);
        }

        /// <summary>
        /// Takes the natural logarithm of the argument.
        /// </summary>
        /// <param name="c">The argument of the logarithm.</param>
        /// <returns>The natural logarithm of the value.</returns>
        public static Complex Log(Complex c)
        {
            var re = Math.Log(Math.Sqrt(c.re * c.re + c.im * c.im));
            var im = c.Arg();
            return new Complex(re, im);
        }

        /// <summary>
        /// Takes the general logarithm of the argument.
        /// </summary>
        /// <param name="c">The argument of the logarithm.</param>
        /// <param name="newBase">The basis to use for the logarithm.</param>
        /// <returns>The general logarithm (in an arbitrary basis) of the value.</returns>
        public static Complex Log(Complex c, double newBase)
        {
            var re = Math.Log(Math.Sqrt(c.re * c.re + c.im * c.im), newBase);
            var im = c.Arg();
            return new Complex(re, im);
        }

        /// <summary>
        /// Takes the square root of the scalar.
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The square root of the value.</returns>
        public static Complex Sqrt(Complex c)
        {
            if (c.re == 0.0 && c.im == 0.0)
                return Complex.Zero;

            var theta = c.re == 0.0 ? Math.PI / 2 * Math.Sign(c.im) : Math.Atan2(c.im, c.re);
            var li = c.re / Math.Cos(theta);
            var alpha = c.re == 0.0 ? 1.0 : Math.Pow(Math.Abs(li), 0.5);
            var beta = theta * 0.5;
            var a = alpha * Math.Cos(beta);
            var b = alpha * Math.Sin(beta);
            return li < 0 ? new Complex(-b, a) : new Complex(a, b);
        }

        /// <summary>
        /// Gives the signum of the scalar.
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The sign of the value.</returns>
        public static Complex Sign(Complex c)
        {
            if (c.re == 0 && c.im == 0.0)
                return Complex.Zero;

            var arg = c.Arg();
            return new Complex(Math.Cos(arg), Math.Sin(arg));
        }

        /// <summary>
        /// Computes the ceiling function of the complex number.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <returns>Real and imaginary parts ceiled.</returns>
        public static Complex Ceil(Complex z)
        {
            var re = Math.Ceiling(z.re);
            var im = Math.Ceiling(z.im);
            return new Complex(re, im);
        }

        /// <summary>
        /// Computes the floor function of the complex number.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <returns>Real and imaginary parts floored.</returns>
        public static Complex Floor(Complex z)
        {
            var re = Math.Floor(z.re);
            var im = Math.Floor(z.im);
            return new Complex(re, im);
        }

        /// <summary>
        /// Rounds the complex number in its real and imaginary part.
        /// </summary>
        /// <param name="z">The argument.</param>
        /// <returns>Real and imaginary parts rounded.</returns>
        public static Complex Round(Complex z)
        {
            return new Complex(Math.Round(z.re), Math.Round(z.im));
        }

        /// <summary>
        /// Computes Gamma(z + 1.0), i.e. the factorial of z.
        /// </summary>
        /// <param name="z">The arugment to get the factorial of.</param>
        /// <returns>The value of Gamma(z + 1.0).</returns>
        public static Complex Factorial(Complex z)
        {
            return Complex.Exp(Gamma.LogGamma(z + 1.0));
        }

        #endregion

        #region Conversions

        public static implicit operator Complex(Int16 real)
        {
            return new Complex(real);
        }

        public static implicit operator Complex(Int32 real)
        {
            return new Complex(real);
        }

        public static implicit operator Complex(Int64 real)
        {
            return new Complex(real);
        }

        public static implicit operator Complex(Single real)
        {
            return new Complex(real);
        }

        public static implicit operator Complex(Double real)
        {
            return new Complex(real);
        }

        public static explicit operator Double(Complex cmplx)
        {
            return Math.Sqrt(cmplx.re * cmplx.re + cmplx.im * cmplx.im);
        }

        public static bool operator true(Complex c)
        {
            return Math.Abs(c.re) > double.Epsilon || Math.Abs(c.im) > double.Epsilon;
        }

        public static bool operator false(Complex c)
        {
            return Math.Abs(c.re) <= double.Epsilon && Math.Abs(c.im) <= double.Epsilon;
        }

        public static explicit operator bool(Complex c)
        {
            return Math.Abs(c.re) > double.Epsilon || Math.Abs(c.im) > double.Epsilon;
        }

        #endregion

        #region Arithmetic operators

        public static Complex operator +(Complex c)
        {
            return new Complex(c.re, c.im);
        }

        public static Complex operator -(Complex c)
        {
            return new Complex(-c.re, -c.im);
        }

        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.re + c2.re, c1.im + c2.im);
        }

        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.re - c2.re, c1.im - c2.im);
        }

        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.re * c2.re - c1.im * c2.im, c1.re * c2.im + c1.im * c2.re);
        }

        public static Complex operator /(Complex c1, Complex c2)
        {
            double nrm = c2.re * c2.re + c2.im * c2.im;
            return new Complex((c1.re * c2.re + c1.im * c2.im) / nrm, (c1.im * c2.re - c1.re * c2.im) / nrm);
        }

        public static Complex operator %(Complex c1, Complex c2)
        {
            return c1 + c2 * Complex.Ceil(-c1 / c2);
        }

        #endregion

        #region Comparison operators

        public static bool operator ==(Complex c1, Complex c2)
        {
            return c1.re == c2.re && c1.im == c2.im;
        }

        public static bool operator !=(Complex c1, Complex c2)
        {
            return c1.re != c2.re || c1.im != c2.im;
        }
        
        public static bool operator <=(Complex c1, Complex c2)
        {
            return c1.Abs() <= c2.Abs();
        }

        public static bool operator >=(Complex c1, Complex c2)
        {
            return c1.Abs() >= c2.Abs();
        }

        public static bool operator <(Complex c1, Complex c2)
        {
            return c1.Abs() < c2.Abs();
        }

        public static bool operator >(Complex c1, Complex c2)
        {
            return c1.Abs() > c2.Abs();
        }

        /// <summary>
        /// Gets the hashcode for this number.
        /// </summary>
        /// <returns>The base hashcode.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Looks if this number is equal to the given object.
        /// </summary>
        /// <param name="o">The object to compare to.</param>
        /// <returns>True if the given object is equal to this number, otherwise false.</returns>
        public override bool Equals(object o)
        {
            if (o is Complex)
                return this == (Complex)o;

            return false;
        }

        #endregion

        #region String representation

        /// <summary>
        /// Returns the complex number as a string.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString(NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Returns the complex number as a formatted string.
        /// </summary>
        /// <param name="format">The format specification.</param>
        /// <param name="formatProvider">A format provider.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var real = re.ToString(format, formatProvider);

            if (im == 0.0)
                return real;

            var imag = im.ToString(format, formatProvider) + "i";

            if (re == 0.0)
                return imag;
            else if (im > 0.0)
                imag = "+ " + imag;

            return real + imag;
        }

        /// <summary>
        /// Returns the complex number as a formatted string.
        /// </summary>
        /// <param name="format">The format specification.</param>
        /// <param name="formatProvider">A format provider.</param>
        /// <returns>The string representation.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var real = re.ToString(formatProvider);

            if (im == 0.0)
                return real;

            var imag = im.ToString(formatProvider) + "i";

            if (re == 0.0)
                return imag;
            else if (im > 0.0)
                imag = "+ " + imag;

            return real + imag;
        }

        #endregion

        #region Trigonometry

        /// <summary>
        /// Takes the sine of the scalar.
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The sine of the value.</returns>
        public static Complex Sin(Complex c)
        {
            var re = Math.Sin(c.re) * Math.Cosh(c.im);
            var im = Math.Cos(c.re) * Math.Sinh(c.im);
            return new Complex(re, im);
        }

        /// <summary>
        /// Takes the cosine of the scalar.
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The cosine of the value.</returns>
        public static Complex Cos(Complex c)
        {
            var re = Math.Cos(c.re) * Math.Cosh(c.im);
            var im = -Math.Sin(c.re) * Math.Sinh(c.im);
            return new Complex(re, im);
        }

        /// <summary>
        /// Takes the tangent (sin / cos) of the scalar.
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The tangent of the value.</returns>
        public static Complex Tan(Complex c)
        {
            return Complex.Sin(c) / Complex.Cos(c);
        }

        /// <summary>
        /// Takes the co-tangent (cos / sin) of the scalar.
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The co-tangent of the value.</returns>
        public static Complex Cot(Complex c)
        {
            return Complex.Cos(c) / Complex.Sin(c);
        }

        public static Complex Sec(Complex z)
        {
            return 1.0 / Complex.Cos(z);
        }

        public static Complex Csc(Complex z)
        {
            return 1.0 / Complex.Sin(z);
        }

        /// <summary>
        /// Computes the inverse sine of the scalar (arcsin).
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The arcsin of the value.</returns>
        public static Complex Arcsin(Complex c)
        {
            var yy = c.im * c.im;
            var rtp = Math.Sqrt(Math.Pow(c.re + 1.0, 2.0) + yy);
            var rtn = Math.Sqrt(Math.Pow(c.re - 1.0, 2.0) + yy);
            var alpha = 0.5 * (rtp + rtn);
            var beta = 0.5 * (rtp - rtn);
            var re = Math.Asin(beta);
            var im = Math.Sign(c.im) * Math.Log(alpha + Math.Sqrt(alpha * alpha - 1.0));
            return new Complex(re, im);
        }

        /// <summary>
        /// Computes the inverse cosine of the scalar (arccos).
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The arccos of the value.</returns>
        public static Complex Arccos(Complex c)
        {
            var yy = c.im * c.im;
            var rtp = Math.Sqrt(Math.Pow(c.re + 1.0, 2.0) + yy);
            var rtn = Math.Sqrt(Math.Pow(c.re - 1.0, 2.0) + yy);
            var alpha = 0.5 * (rtp + rtn);
            var beta = 0.5 * (rtp - rtn);
            var re = Math.Acos(beta);
            var im = -Math.Sign(c.im) * Math.Log(alpha + Math.Sqrt(alpha * alpha - 1.0));
            return new Complex(re, im);
        }

        /// <summary>
        /// Computes the inverse tangent of the scalar (arctan).
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The arctan of the value.</returns>
        public static Complex Arctan(Complex c)
        {
            var xx = c.re * c.re;
            var yy = c.im * c.im;
            var re = 0.5 * Math.Atan2(2.0 * c.re, 1.0 - xx - yy);
            var im = 0.25 * Math.Log((xx + Math.Pow(c.im + 1.0, 2.0)) / (xx + Math.Pow(c.im - 1.0, 2.0)));
            return new Complex(re, im);
        }

        /// <summary>
        /// Computes the inverse cotangent of the scalar (arccot).
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The arccot of the value.</returns>
        public static Complex Arccot(Complex c)
        {
            return Complex.Arctan(1 / c);
        }

        /// <summary>
        /// Computes the inverse secant of the scalar (arcsec).
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The arcsec of the value.</returns>
        public static Complex Arcsec(Complex c)
        {
            return Complex.Arccos(1 / c);
        }

        /// <summary>
        /// Computes the inverse cosecant of the scalar (arccsc).
        /// </summary>
        /// <param name="c">The complex number.</param>
        /// <returns>The arccsc of the value.</returns>
        public static Complex Arccsc(Complex c)
        {
            return Complex.Arcsin(1.0 / c);
        }

        #endregion

        #region Hyperbolic functions

        /// <summary>
        /// Computes the hyperbolic sine.
        /// </summary>
        /// <param name="z">The complex argument.</param>
        /// <returns>The value of the hyperbolic sine.</returns>
        public static Complex Sinh(Complex z)
        {
            return (Complex.Exp(z) - Complex.Exp(-z)) / 2.0;
        }

        /// <summary>
        /// Computes the hyperbolic cosine.
        /// </summary>
        /// <param name="z">The complex argument.</param>
        /// <returns>The value of the hyperbolic cosine.</returns>
        public static Complex Cosh(Complex z)
        {
            return (Complex.Exp(z) + Complex.Exp(-z)) / 2.0;
        }

        /// <summary>
        /// Computes the hyperbolic tangent.
        /// </summary>
        /// <param name="z">The complex argument.</param>
        /// <returns>The value of the hyperbolic tangent.</returns>
        public static Complex Tanh(Complex z)
        {
            var a = Complex.Exp(z);
            var b = Complex.Exp(-z);
            return (a - b) / (a + b);
        }

        /// <summary>
        /// Computes the hyperbolic cotangent.
        /// </summary>
        /// <param name="z">The complex argument.</param>
        /// <returns>The value of the hyperbolic cotangent.</returns>
        public static Complex Coth(Complex z)
        {
            var a = Complex.Exp(z);
            var b = Complex.Exp(-z);
            return (a + b) / (a - b);
        }

        public static Complex Sech(Complex z)
        {
            return 2.0 / (Complex.Exp(z) + Complex.Exp(-z));
        }

        public static Complex Csch(Complex z)
        {
            return 2.0 / (Complex.Exp(z) - Complex.Exp(-z));
        }

        public static Complex Arsinh(Complex z)
        {
            return Complex.Ln(z + Complex.Sqrt((z * z) + 1.0));
        }

        public static Complex Arcosh(Complex z)
        {
            return Complex.Ln(z + Complex.Sqrt((z * z) - 1.0));
        }

        public static Complex Artanh(Complex z)
        {
            return 0.5 * Complex.Ln((1.0 + z) / (1.0 - z));
        }

        public static Complex Arcoth(Complex z)
        {
            return 0.5 * Complex.Ln((1.0 + z) / (z - 1.0));
        }

        public static Complex Arsech(Complex z)
        {
            var zi = 1.0 / z;
            return Complex.Ln(zi + Complex.Sqrt(zi + 1.0) * Complex.Sqrt(zi - 1.0));
        }

        public static Complex Arcsch(Complex z)
        {
            return Complex.Ln(1.0 / z + Complex.Sqrt(1.0 / (z * z) + 1.0));
        }

        #endregion

        #region Interface implementations

        public bool Equals(Complex other)
        {
            return this == other;
        }

        public int CompareTo(Complex other)
        {
            if (this > other)
                return 1;
            else if (this < other)
                return -1;

            return 0;
        }

        public int CompareTo(object obj)
        {
            if (obj is Complex)
                return CompareTo((Complex)obj);

            return -1;
        }

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return (bool)this;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte)(double)this;
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (char)(double)this;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal)(double)this;
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double)this;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short)(double)this;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return (int)(double)this;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (long)(double)this;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte)(double)this;
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float)(double)this;
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(Complex))
                return this;
            else if (conversionType == typeof(Double))
                return (Double)this;
            else if (conversionType == typeof(String))
                return this.ToString();
            else if (conversionType == typeof(Boolean))
                return (Boolean)this;

            return null;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return (ushort)(double)this;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return (uint)(double)this;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return (ulong)(double)this;
        }

        #endregion
    }
}
