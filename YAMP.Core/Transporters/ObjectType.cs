using System;
using YAMP.Core;

namespace YAMP.Types
{
    sealed class ObjectType : IType
    {
        public TypeMetric RelationTo(IType type)
        {
            if (type is IObject)
                return TypeMetric.Exact;

            return TypeMetric.Derived;
        }

        public Object Convert(Dynamic obj)
        {
            return obj.Value;
        }

        public Boolean IsTrue(Object obj)
        {
            return obj != null;
        }

        public Boolean IsType(Object value)
        {
            return true;
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
            get { return "Object"; }
        }

        public Boolean AreEqual(Object f1, Object f2)
        {
            return Object.ReferenceEquals(f1, f2);
        }

        public Boolean AreNotEqual(Object f1, Object f2)
        {
            return !Object.ReferenceEquals(f1, f2);
        }
    }
}
