using System;
using System.Globalization;
using YAMP.Core;

namespace YAMP.Types
{
    sealed class ComplexType : IType, IAddable, ITransposable, IPowable, IMeasurable
    {
        public TypeMetric RelationTo(IType type)
        {
            if (type is ComplexType)
                return TypeMetric.Exact;
            else if (type is RealType || type is IntegerType || type is BooleanType)
                return TypeMetric.Derived;

            return TypeMetric.None;
        }

        public Object Convert(Dynamic obj)
        {
            if (obj.Type is ComplexType)
                return obj.Value;
            else if (obj.Type is RealType)
                return (Complex)(Double)obj.Value;
            else if (obj.Type is IntegerType)
                return (Complex)(Int64)obj.Value;
            else if (obj.Type is BooleanType)
                return (Boolean)obj.Value ? Complex.One : Complex.Zero;

            return Complex.Zero;
        }

        public Boolean IsType(Object value)
        {
            return value is Complex;
        }

        public String ToString(Object instance)
        {
            return ((Complex)instance).ToString(NumberFormatInfo.InvariantInfo);
        }

        public String ToCode(Object instance)
        {
            return ((Complex)instance).ToString(NumberFormatInfo.InvariantInfo);
        }

        public String Name
        {
            get { return "Complex"; }
        }

        public Boolean IsTrue(Object o)
        {
            return (Complex)o != 0.0;
        }

        public Object Adjungate(Object z)
        {
            return ((Complex)z).Conj();
        }

        public Object Transpose(Object z)
        {
            return z;
        }

        public Object Negate(Object z)
        {
            return -((Complex)z);
        }

        public Object Factorial(Object z)
        {
            return Complex.Factorial((Complex)z);
        }

        public Object Add(Object x, Object y)
        {
            return (Complex)x + (Complex)y;
        }

        public Object Div(Object x, Object y)
        {
            return (Complex)x / (Complex)y;
        }

        public Object Mod(Object x, Object y)
        {
            return (Complex)x % (Complex)y;
        }

        public Object Mul(Object x, Object y)
        {
            return (Complex)x * (Complex)y;
        }

        public Object Pow(Object x, Object y)
        {
            return Complex.Pow((Complex)x, (Complex)y);
        }

        public Object Sub(Object x, Object y)
        {
            return (Complex)x - (Complex)y;
        }

        public Boolean AreEqual(Object x, Object y)
        {
            return (Complex)x == (Complex)y;
        }

        public Boolean AreNotEqual(Object x, Object y)
        {
            return (Complex)x != (Complex)y;
        }

        public Object GreaterEqual(Object x, Object y)
        {
            return (Complex)x >= (Complex)y;
        }

        public Object LessEqual(Object x, Object y)
        {
            return (Complex)x <= (Complex)y;
        }

        public Object GreaterThan(Object x, Object y)
        {
            return (Complex)x > (Complex)y;
        }

        public Object LessThan(Object x, Object y)
        {
            return (Complex)x < (Complex)y;
        }
    }
}
