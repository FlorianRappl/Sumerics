using System;
using YAMP.Core;

namespace YAMP.Types
{
    class ArrayType : IType
    {
        IType _atomic;

        public ArrayType(IType atomic)
        {
            _atomic = atomic;
        }

        public TypeMetric RelationTo(IType type)
        {
            return TypeMetric.None;
        }

        public Object Convert(Dynamic obj)
        {
            var wrapper = new DynamicList();
            wrapper.Add(obj);
            return wrapper;
        }

        public Boolean IsTrue(Object obj)
        {
            var arr = (DynamicList)obj;
            return arr.Count != 0;
        }

        public String Name
        {
            get { return _atomic.Name + "[]"; }
        }

        public Boolean IsType(Object value)
        {
            return value is DynamicList && ((DynamicList)value).Type == _atomic;
        }

        public Boolean AreEqual(Object left, Object right)
        {
            return Object.ReferenceEquals(left, right);
        }

        public Boolean AreNotEqual(Object left, Object right)
        {
            return !Object.ReferenceEquals(left, right);
        }

        public String ToCode(Object value)
        {
            var arr = (DynamicList)value;
            var values = new String[arr.Count];

            for (int i = 0; i < values.Length; i++)
                values[i] = arr[i].Codify();

            return "[" + String.Join(", ", values) + "]";
        }

        public String ToString(Object value)
        {
            var arr = (DynamicList)value;
            var values = new String[arr.Count];

            for (int i = 0; i < values.Length; i++)
                values[i] = arr[i].Stringify();

            return String.Join(",", values);
        }
    }
}
