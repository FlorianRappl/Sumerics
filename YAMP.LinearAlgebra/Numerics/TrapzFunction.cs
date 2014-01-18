using System;
using YAMP.Attributes;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.LinearAlgebra
{
    [Kind(KindAttribute.FunctionKind.Mathematics)]
	[Description("Integrates a given vector of values with the Trapezoidal rule (perfect for first order polynomials, i.e. linear functions).")]
    [Link("http://en.wikipedia.org/wiki/Trapezoidal_rule")]
    sealed class TrapzFunction : YFunction
    {
        [Description("Computes the integral for a given range of y values with its x vector.")]
        [Example("trapz(sin(0:0.1:Pi), 0:0.1:Pi)", "Computes the value of the sinus function between 0 and pi (analytic result is 2).")]
        public Double Invoke(Matrix y, Matrix x)
        {
            var integral = new TrapezIntegrator(y.ToRealArray());
            return integral.Integrate(x.ToRealArray());
        }
    }
}
