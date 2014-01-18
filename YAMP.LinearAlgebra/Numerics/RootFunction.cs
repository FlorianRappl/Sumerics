using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;
using YAMP.Types;

namespace YAMP.LinearAlgebra
{
    [Kind(KindAttribute.FunctionKind.Mathematics)]
	[Description("A root-finding algorithm is a numerical method, or algorithm, for finding a value x such that f(x) = 0, for a given function f. Such an x is called a root of the function f.")]
    [Link("http://en.wikipedia.org/wiki/Zero_of_a_function")]
    sealed class RootFunction : YFunction
	{
		[Description("This function calls finds the root of f(x) that is the closest to the given value of x. The output is a value x0, which has the property that f(x0) = 0. There might be more roots depending on the starting value of x.")]
		[Example("root(x => x^2+x, -2)", "Returns the value of -1, which has been found as a root of the function f(x) = x^2 + x. The starting value of x has been chosen to -2.")]
		[Example("root(x => x^2+x, 1)", "Returns the value of 0, which has been found as a root of the function f(x) = x^2 + x. The starting value of x has been chosen to 0.")]
        public Double Invoke(RunContext ctx, IFunction f, Double x)
		{
            var o = new object[1];
            o[0] = 2.1;
            var g = f.Resolver.Resolve(o);

			Func<double, double> lambda = t =>
			{
                o[0] = t;
                return (Double)RealType.Instance.Cast(g(ctx, o));
			};

			var newton = new NewtonMethod(lambda, x, 0.00001);
            return newton.Result[0, 0];
		}
	}
}
