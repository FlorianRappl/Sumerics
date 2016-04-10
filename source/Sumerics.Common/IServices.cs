namespace Sumerics
{
    using System;
    using System.Collections.Generic;

    public interface IServices
    {
        Object Get(Type type);

        IEnumerable<Object> All(Type type);
    }
}
