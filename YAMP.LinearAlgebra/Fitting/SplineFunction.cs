using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind("LinearAlgebra")]
	[Description("Interpolates points between given sample values.")]
    [Link("http://en.wikipedia.org/wiki/Spline_(mathematics)")]
    sealed class SplineFunction : YFunction
    {
        [Description("Generates the y value for a single point with given sample data.")]
        [Example("spline([-3,9;-2,4;-1,1;0,0;1,1;3,9], 2)", "Interpolates the y value for x = 2 in this quadratic function. The final outcome is slightly greater than 4.")]
        public Double[] Invoke(Matrix original, Double x)
        {
            var X = original.GetColumnVector(1).ToRealArray();
            var Y = original.GetColumnVector(2).ToRealArray();
            var spline = new SplineInterpolation(X, Y);
            return new double[] { x, spline.ComputeValue(x) };
        }

        [Description("Generates the y values for a vector of points with given sample data.")]
        [Example("spline([-3,9;-2,4;-1,1;0,0;1,1;3,9], [-1.5, -0.5, 0, 0.5, 1.5])", "Interpolates the y values for this x vector in the quadratic function. The final outcome is a small derivation of the real values.")]
        public Matrix Invoke(Matrix original, Matrix x)
        {
            var X = original.GetColumnVector(1).ToRealArray();
            var Y = original.GetColumnVector(2).ToRealArray();
            var spline = new SplineInterpolation(X, Y);
            return spline.ComputeValues(x);
        }
    }
}
