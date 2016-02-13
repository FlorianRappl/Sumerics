namespace Sumerics
{
    using System;
    using System.Collections.Generic;

    public interface IComponents
    {
        Object Create(Type type);

        Object Get(Type type);

        IEnumerable<Object> All(Type type);
    }
}
