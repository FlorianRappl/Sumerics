using System;
using YAMP.Core;

namespace YAMP.Types
{
    sealed class BooleanType : IType, IAddable, IPowable
    {
        public TypeMetric RelationTo(IType type)
        {
            if (type is BooleanType)
                return TypeMetric.Exact;

            return TypeMetric.Castable;
        }

        public Object Convert(Dynamic obj)
        {
            if (obj.Type is BooleanType)
                return obj.Value;

            return obj.Type.IsTrue(obj.Value);
        }

        public String Name
        {
            get { return "Boolean"; }
        }

        public Boolean IsType(Object value)
        {
            return value is Boolean;
        }

        public String ToString(Object instance)
        {
            return (Boolean)instance ? "true" : "false";
        }

        public String ToCode(Object instance)
        {
            return (Boolean)instance ? "true" : "false";
        }

        public Boolean IsTrue(Object o)
        {
            return (Boolean)o;
        }

        public Boolean AreEqual(Object x, Object y)
        {
            return (Boolean)x == (Boolean)y;
        }

        public Boolean AreNotEqual(Object x, Object y)
        {
            return (Boolean)x != (Boolean)y;
        }

        public Object Negate(Object instance)
        {
            return !(Boolean)instance;
        }

        public Object Factorial(Object instance)
        {
            return true;
        }

        public Object Add(Object a, Object b)
        {
            return (Boolean)a || (Boolean)b;
        }

        public Object Div(Object a, Object b)
        {
            return !((Boolean)a && (Boolean)b);
        }

        public Object Mod(Object left, Object right)
        {
            return !(Boolean)right;
        }

        public Object Mul(Object a, Object b)
        {
            return (Boolean)a && (Boolean)b;
        }

        public Object Pow(Object a, Object b)
        {
            return (Boolean)a;
        }

        public Object Sub(Object a, Object b)
        {
            return !((Boolean)a || (Boolean)b);
        }
    }
}
