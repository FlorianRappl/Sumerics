using System;
using YAMP.Core;

namespace YAMP.Types
{
    sealed class StringType : IType, IAddable, IMeasurable
    {
        public TypeMetric RelationTo(IType type)
        {
            if (type is StringType)
                return TypeMetric.Exact;

            return TypeMetric.Castable;
        }

        public Object Convert(Dynamic value)
        {
            return value.Stringify();
        }

        public String Name
        {
            get { return "String"; }
        }

        public Boolean IsType(Object value)
        {
            return value is String;
        }

        public Boolean IsTrue(Object o)
        {
            return !String.IsNullOrEmpty((String)o);
        }

        public Boolean AreEqual(Object x, Object y)
        {
            return (String)x == (String)y;
        }

        public Boolean AreNotEqual(Object x, Object y)
        {
            return (String)x != (String)y;
        }

        public Object GreaterEqual(Object x, Object y)
        {
            return ((String)x).Length >= ((String)y).Length;
        }

        public Object LessEqual(Object x, Object y)
        {
            return ((String)x).Length <= ((String)y).Length;
        }

        public Object GreaterThan(Object x, Object y)
        {
            return ((String)x).Length > ((String)y).Length;
        }

        public Object LessThan(Object x, Object y)
        {
            return ((String)x).Length < ((String)y).Length;
        }

        public String ToString(Object value)
        {
            return value.ToString();
        }

        public String ToCode(Object value)
        {
            return "\"" + value.ToString() + "\"";
        }

        public Object Negate(Object instance)
        {
            var charArray = ((String)instance).ToCharArray();
            Array.Reverse(charArray);
            return new String(charArray);
        }

        public Object Add(Object a, Object b)
        {
            return (String)a + (String)b;
        }

        public Object Sub(Object a, Object b)
        {
            return ((String)a).Replace((String)b, String.Empty);
        }
    }
}
