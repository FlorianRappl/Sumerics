namespace Sumerics
{
    using System;
    using System.Collections.Generic;

    public interface IComponents
    {
        Object Get(Type type);

        IEnumerable<Object> All(Type type);
    }
}
