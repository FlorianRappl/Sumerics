using System;
using YAMP.Core;

namespace YAMP.Types
{
    sealed class FunctionType : IType, ICallable
    {
        public TypeMetric RelationTo(IType type)
        {
            if (type is FunctionType)
                return TypeMetric.Exact;

            return TypeMetric.None;
        }

        public Object Convert(Dynamic o)
        {
            return o.Value;
        }

        public Boolean IsTrue(Object o)
        {
            return o != null;
        }

        public Boolean IsType(Object value)
        {
            return value is IFunction;
        }

        public String ToString(Object instance)
        {
            return instance.ToString();
        }

        public String ToCode(Object instance)
        {
            return instance.ToString();
        }

        public String Name
        {
            get { return "Function"; }
        }

        public Boolean AreEqual(Object f1, Object f2)
        {
            return Object.ReferenceEquals(f1, f2);
        }

        public Boolean AreNotEqual(Object f1, Object f2)
        {
            return !Object.ReferenceEquals(f1, f2);
        }

        public Dynamic Invoke(Object f, Dynamic[] args)
        {
            var h = (IFunction)f;
            var g = h.Resolver.Resolve(args);

            if (g == null)
                throw new ArgumentException("The given parameters cannot be used for the " + h.Name + " function.");

            return g(args);
        }
    }
}
