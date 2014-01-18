using System;
using YAMP.Core;

namespace YAMP.Types
{
    sealed class ExceptionType : IType
    {
        public TypeMetric RelationTo(IType type)
        {
            if (type is ExceptionType)
                return TypeMetric.Exact;

            return TypeMetric.None;
        }

        public Object Convert(Dynamic obj)
        {
            return obj.Value;
        }

        public Boolean IsTrue(Object obj)
        {
            return false;
        }

        public String Name
        {
            get { return "Exception"; }
        }

        public Boolean AreEqual(Object left, Object right)
        {
            return ((Exception)left).Message == ((Exception)right).Message;
        }

        public Boolean AreNotEqual(Object left, Object right)
        {
            return ((Exception)left).Message != ((Exception)right).Message;
        }

        public Boolean IsType(Object value)
        {
            return value is Exception;
        }

        public String ToString(Object value)
        {
            return ((Exception)value).Message;
        }

        public String ToCode(Object value)
        {
            return ((Exception)value).Message;
        }
    }
}
