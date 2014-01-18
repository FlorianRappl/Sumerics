using System;
using YAMP.Core;

namespace YAMP.Types
{
    sealed class IntegerType : IType, IAddable, IMeasurable, IPowable
    {
        public TypeMetric RelationTo(IType type)
        {
            if (type is IntegerType)
                return TypeMetric.Exact;
            else if (type is BooleanType)
                return TypeMetric.Derived;

            return TypeMetric.None;
        }

        public Object Convert(Dynamic obj)
        {
            if (obj.Type is IntegerType)
                return obj.Value;
            else if (obj.Type is BooleanType)
                return (Boolean)obj.Value ? 1L : 0L;

            return 0L;
        }

        public Boolean IsType(Object value)
        {
            return value is Int64;
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
            get { return "Integer"; }
        }

        public Boolean IsTrue(Object o)
        {
            return (Int64)o != 0L;
        }

        public Boolean AreEqual(Object x, Object y)
        {
            return (Int64)x == (Int64)y;
        }

        public Boolean AreNotEqual(Object x, Object y)
        {
            return (Int64)x != (Int64)y;
        }

        public Object Negate(Object x)
        {
            return -(Int64)x;
        }

        public Object Factorial(Object x)
        {
            var p = (Int64)x;
            var res = 1L;

            while (p > 1)
                res *= p--;

            return res;
        }

        public Object Add(Object x, Object y)
        {
            return (Int64)x + (Int64)y;
        }

        public Object Div(Object x, Object y)
        {
            return (Double)((Int64)x) / (Double)((Int64)y);
        }

        public Object Mod(Object x, Object y)
        {
            return (Int64)x % (Int64)y;
        }

        public Object Mul(Object x, Object y)
        {
            return (Int64)x * (Int64)y;
        }

        public Object Pow(Object x, Object y)
        {
            return Math.Pow((Double)((Int64)x), (Double)((Int64)y));
        }

        public Object Sub(Object x, Object y)
        {
            return (Int64)x - (Int64)y;
        }

        public Object GreaterEqual(Object x, Object y)
        {
            return (Int64)x >= (Int64)y;
        }

        public Object LessEqual(Object x, Object y)
        {
            return (Int64)x <= (Int64)y;
        }

        public Object GreaterThan(Object x, Object y)
        {
            return (Int64)x > (Int64)y;
        }

        public Object LessThan(Object x, Object y)
        {
            return (Int64)x < (Int64)y;
        }
    }
}
