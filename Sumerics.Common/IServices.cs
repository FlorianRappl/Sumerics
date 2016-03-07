namespace Sumerics
{
    using System;
    using System.Collections.Generic;

    public interface IServices
    {
        Object Create(Type type);

        Object Get(Type type);

        IEnumerable<Object> All(Type type);
    }
}
