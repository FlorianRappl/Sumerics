using System;

namespace YAMP.Core
{
    interface IAddable : IType
    {
        Object Negate(Object instance);

        Object Add(Object left, Object right);

        Object Sub(Object left, Object right);
    }
}
