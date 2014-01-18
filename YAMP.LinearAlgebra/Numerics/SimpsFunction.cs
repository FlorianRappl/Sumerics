using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind(KindAttribute.FunctionKind.Mathematics)]
	[Description("Integrates a given vector of values with Simpson's rule (perfect for third order polynomials, i.e. cubic functions).")]
    [Link("http://en.wikipedia.org/wiki/Simpson's_rule")]
    sealed class SimpsFunction : YFunction
    {
        [Description("Computes the integral for a given range of y values with its x vector.")]
        [Example("simps(sin(0:0.1:Pi), 0:0.1:Pi)", "Computes the value of the sinus function between 0 and pi (analytic result is 2).")]
        public Double Invoke(Matrix y, Matrix x)
        {
            var integral = new SimpsonIntegrator(y.ToRealArray());
            return integral.Integrate(x.ToRealArray());
        }
    }
}
