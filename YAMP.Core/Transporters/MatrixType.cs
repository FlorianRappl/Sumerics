using System;
using System.Collections.Generic;
using YAMP.Core;

namespace YAMP.Types
{
    sealed class MatrixType : IType, ITransposable, IMeasurable, IPowable
    {
        public TypeMetric RelationTo(IType type)
        {
            if (type is MatrixType)
                return TypeMetric.Exact;
            else if (type is ComplexType)
                return TypeMetric.Castable;
            else if (type is IntegerType)
                return TypeMetric.Castable;
            else if (type is RealType)
                return TypeMetric.Castable;

            return TypeMetric.None;
        }

        public Object Convert(Dynamic o)
        {
            if (o.Type is MatrixType)
                return o.Value;
            else
            {
                var M = new Matrix(1, 1);

                if (o.Type is ComplexType)
                    M[0, 0] = (Complex)o.Value;
                else if (o.Type is IntegerType)
                    M[0, 0] = (Int64)o.Value;
                else if (o.Type is RealType)
                    M[0, 0] = (Double)o.Value;

                return M;
            }
        }

        public Boolean IsTrue(Object o)
        {
            return ((Matrix)o).HasElements;
        }

        public Boolean IsType(Object value)
        {
            return value is Matrix;
        }

        public String Name
        {
            get { return "Matrix"; }
        }

        public Boolean AreEqual(Object left, Object right)
        {
            return ((Matrix)left).Equals((Matrix)right);
        }

        public Boolean AreNotEqual(Object left, Object right)
        {
            return !((Matrix)left).Equals((Matrix)right);
        }

        public String ToString(Object value)
        {
            return ((Matrix)value).ToString();
        }

        public String ToCode(Object value)
        {
            return ((Matrix)value).ToCode();
        }

        public Object Adjungate(Object x)
        {
            return ((Matrix)x).Adj();
        }

        public Object Transpose(Object x)
        {
            return ((Matrix)x).Trans();
        }

        public Object GreaterEqual(Object A, Object B)
        {
            return (Matrix)A >= (Matrix)B;
        }

        public Object LessEqual(Object A, Object B)
        {
            return (Matrix)A <= (Matrix)B;
        }

        public Object GreaterThan(Object A, Object B)
        {
            return (Matrix)A > (Matrix)B;
        }

        public Object LessThan(Object A, Object B)
        {
            return (Matrix)A < (Matrix)B;
        }

        public Object Negate(Object M)
        {
            return -(Matrix)M;
        }

        public Object Factorial(Object M)
        {
            return ((Matrix)M).ForEach(z => Complex.Factorial(z));
        }

        public Object Add(Object a, Object b)
        {
            var A = (Matrix)a;
            var B = (Matrix)b;

            if (A.Length == 1)
                return A[0, 0] * B;
            else if (B.Length == 1)
                return A * B[0, 0];

            return A + B;
        }

        public Object Div(Object a, Object b)
        {
            var A = (Matrix)a;
            var B = (Matrix)b;

            if (A.Length == 1)
                return A[0, 0] / B;
            else if (B.Length == 1)
                return A / B[0, 0];

            return A / B;
        }

        public Object Mod(Object A, Object B)
        {
            throw new ArgumentException("The % operator is not supported with matrices.");
        }

        public Object Mul(Object a, Object b)
        {
            var A = (Matrix)a;
            var B = (Matrix)b;

            if (A.Length == 1)
                return A[0, 0] * B;
            else if (B.Length == 1)
                return A * B[0, 0];

            return A * B;
        }

        public Object Pow(Object a, Object b)
        {
            var A = (Matrix)a;
            var B = (Matrix)b;

            if (A.Length == 1)
                return Matrix.Pow(A[0, 0], B);
            else if (B.Length == 1)
                return Matrix.Pow(A, B[0, 0]);

            throw new ArgumentException("The ^ operator is not supported with matrices.");
        }

        public Object Sub(Object a, Object b)
        {
            var A = (Matrix)a;
            var B = (Matrix)b;

            if (A.Length == 1)
                return A[0, 0] - B;
            else if (B.Length == 1)
                return A - B[0, 0];

            return A - B;
        }
    }
}
