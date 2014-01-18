using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;
using YAMP.Types;

namespace YAMP.LinearAlgebra
{
    [Kind(KindAttribute.FunctionKind.Mathematics)]
	[Description("Solves one dimensional ordinary differential equations in the form x'(t) = f(t, x(t)).")]
    [Link("http://en.wikipedia.org/wiki/Ordinary_differential_equation")]
    sealed class OdeFunction : YFunction
	{
		[Description("Searches for a solution of the differential equation x'(t) = f(t, x) for a given a lambda expression f with two arguments t and x within the range specified as a vector and the starting value of x(t) at t(0) (the first value for t).")]
		[Example("ode((t, x) => -x, 0:0.01:2, 1)", "Solves the DEQ x'(t) + x(t) = 0 and gets the solution vector, which is exp(-t) within the specified point range.")]
		[Example("ode((t, x) => x - t, 0:0.01:5, 1.5)", "Solves the DEQ x'(t) = x(t) - t and gets the solution vector, which is 1 / 2 * exp(t) + t + 1 within the specified point range.")]
        public Matrix Invoke(RunContext ctx, IFunction deq, Double[] points, Double x0)
		{
            var o = new object[2];
            o[0] = 1.0;
            o[1] = 1.0;
            var f = deq.Resolver.Resolve(o);

			Func<double, double, double> lambda = (t, x) => 
			{
                o[0] = t;
                o[1] = x;
				return (Double)RealType.Instance.Cast(f(ctx, o));
			};

			var euler = new RungeKutta(lambda, points[0], points[points.Length - 1], x0, points.Length - 1);
            return new Matrix(euler.Result, points.Length, 2);
		}
	}
}
