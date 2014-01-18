using System;

namespace YAMP.Core
{
    interface IObject
    {
        String[] Methods { get; }
        String[] Properties { get; }

        CustomFunction GetMethod(String name);
        Boolean HasMethod(String name);

        Boolean HasProperty(String name);
        Boolean TryReadProperty(String name, out Dynamic value);
        Boolean TryWriteProperty(String name, Dynamic value);
    }
}
