using System;

namespace YAMP.Core
{
    interface IPowable : IType
    {
        Object Pow(Object left, Object right);

        Object Mul(Object left, Object right);

        Object Div(Object left, Object right);

        Object Mod(Object left, Object right);

        Object Factorial(Object instance);
    }
}
