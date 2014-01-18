using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
    [Description("The function provides polynomial evaluation. In mathematics, a polynomial is an expression of finite length constructed from variables (also called indeterminates) and constants, using only the operations of addition, subtraction, multiplication, and non-negative integer exponents. However, the division by a constant is allowed, because the multiplicative inverse of a non zero constant is also a constant.")]
    [Link("http://en.wikipedia.org/wiki/Polynomial")]
    sealed class PolyvalFunction : YFunction
    {
        [Description("The function returns the value of a polynomial of degree n evaluated at X. The input argument p is a vector of length n + 1 whose elements are the coefficients in ascending powers of the polynomial to be evaluated.")]
        [Example("polyval([1 2 3], [5 7 9])", "The polynomial p(x) = 3 * x^2 + 2 * x + 1 is evaluated at x = 5, 7, and 9 with the result 86, 162, 262.")]
        public Matrix Invoke(Matrix p, Matrix M)
        {
            var coeff = p.ToArray();
            var poly = BuildPolynom(coeff);
            return M.ForEach(z => poly(z.Re));
        }

        [Description("The function returns the value of a polynomial of degree n evaluated at z. The input argument p is a vector of length n + 1 whose elements are the coefficients in ascending powers of the polynomial to be evaluated.")]
        [Example("polyval([1 2 3], 5)", "The polynomial p(z) = 3 * z^2 + 2 * z + 1 is evaluated at z = 5 with the result 86.")]
        public Double Invoke(Matrix p, Double x)
        {
            var coeff = p.ToArray();
            var poly = BuildPolynom(coeff);
            return poly(x);
        }

        Func<Double, Double> BuildPolynom(Complex[] coeff)
        {
            return z =>
            {
                var pow = 1.0;
                var sum = 0.0;

                for (var i = 0; i < coeff.Length; i++)
                {
                    var c = coeff[i];
                    sum += c.Re * pow;
                    pow *= z;
                }

                return sum;
            };
        }
    }
}
