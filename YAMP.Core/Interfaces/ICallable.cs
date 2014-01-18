using System;

namespace YAMP.Core
{
    interface ICallable : IType
    {
        Dynamic Invoke(Object f, Dynamic[] args);
    }
}
