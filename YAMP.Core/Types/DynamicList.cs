using System;
using System.Collections.Generic;
using YAMP.Core;

namespace YAMP
{
    class DynamicList : List<Dynamic>
    {
        public IType Type
        {
            get { return Dynamic.FindType(this); }
        }

        public void Flatten(IType desired)
        {
            for (int i = 0; i < Count; i++)
                this[i] = new Dynamic(desired.Convert(this[i]), desired);
        }

        public Object[] ToArguments()
        {
            var args = new Object[Count];

            for (int i = 0; i < Count; i++)
                args[i] = this[i].Value;

            return args;
        }
    }
}
