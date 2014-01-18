using System;
using YAMP.Core;

namespace YAMP.Parser
{
    sealed class Backend
    {
        MathQuery _query;

        public Backend(MathQuery query)
        {
            _query = query;
        }

        public void Optimize(Optimization optimization)
        {
            if (optimization == Optimization.None)
                return;

            //TODO

            if (optimization == Optimization.Combine)
                return;

            //TODO

            if (optimization == Optimization.CombineUnrollInline)
                return;

            //TODO
        }
    }
}
