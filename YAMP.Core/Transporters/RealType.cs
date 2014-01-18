using System;
using System.Globalization;
using YAMP.Core;
using YAMP.Numerics;

namespace YAMP.Types
{
    sealed class RealType : IType, IAddable, IMeasurable, IPowable
    {
        public TypeMetric RelationTo(IType type)
        {
            if (type is RealType)
                return TypeMetric.Exact;
            else if (type is IntegerType || type is BooleanType)
                return TypeMetric.Derived;

            return TypeMetric.None;
        }

        public Object Convert(Dynamic obj)
        {
            if (obj.Value is Double)
                return (Double)obj.Value;
            else if (obj.Value is Int64)
                return (Double)(Int64)obj.Value;
            else if (obj.Value is Boolean)
                return (Boolean)obj.Value ? 1.0 : 0.0;

            return 0.0;
        }

        public String Name
        {
            get { return "Real"; }
        }

        public Boolean IsType(Object value)
        {
            return value is Double;
        }

        public String ToString(Object instance)
        {
            return ((Double)instance).ToString(NumberFormatInfo.InvariantInfo);
        }

        public String ToCode(Object instance)
        {
            return ((Double)instance).ToString(NumberFormatInfo.InvariantInfo);
        }

        public Boolean IsTrue(Object o)
        {
            return (Double)o != 0.0;
        }

        public Boolean AreEqual(Object x, Object y)
        {
            return (Double)x == (Double)y;
        }

        public Boolean AreNotEqual(Object x, Object y)
        {
            return (Double)x != (Double)y;
        }

        public Object Negate(Object x)
        {
            return -(Double)x;
        }

        public Object Factorial(Object x)
        {
            return Helpers.Factorial((Double)x);
        }

        public Object Add(Object x, Object y)
        {
            return (Double)x + (Double)y;
        }

        public Object Div(Object x, Object y)
        {
            return (Double)x / (Double)y;
        }

        public Object Mod(Object x, Object y)
        {
            return (Double)x % (Double)y;
        }

        public Object Mul(Object x, Object y)
        {
            return (Double)x * (Double)y;
        }

        public Object Pow(Object x, Object y)
        {
            return Math.Pow((Double)x, (Double)y);
        }

        public Object Sub(Object x, Object y)
        {
            return (Double)x - (Double)y;
        }

        public Object GreaterEqual(Object x, Object y)
        {
            return (Double)x >= (Double)y;
        }

        public Object LessEqual(Object x, Object y)
        {
            return (Double)x <= (Double)y;
        }

        public Object GreaterThan(Object x, Object y)
        {
            return (Double)x > (Double)y;
        }

        public Object LessThan(Object x, Object y)
        {
            return (Double)x < (Double)y;
        }
    }
}
