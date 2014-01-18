using System;
using YAMP.Core;

namespace YAMP.Types
{
    public sealed class RangeType : BaseType<Range>, IType
    {
        static RangeType instance;

        private RangeType()
        {
        }

        public static RangeType Instance
        {
            get { return instance ?? (instance = new RangeType()); }
        }

        public bool IsIndirect(object obj)
        {
            return false;
        }

        public bool IsTrue(object o)
        {
            return o != null;
        }

        public object Cast(object o)
        {
            if (o is Range)
                return (Range)o;

            return null;
        }

        public object Default()
        {
            return new Range(0, 0);
        }

        public string Name
        {
            get { return "Range"; }
        }

        public void RegisterOperators(IAddBinary register)
        {
            register.Addition((x, y) => (Range)x + (Double)y, this, RealType.Instance);
            register.Addition((x, y) => (Range)y + (Double)x, RealType.Instance, this);
            register.Subtract((x, y) => (Range)x - (Double)y, this, RealType.Instance);
            register.Subtract((x, y) => (Double)x - (Range)y, RealType.Instance, this);
            register.Multiply((x, y) => (Range)x * (Double)y, this, RealType.Instance);
            register.Multiply((x, y) => (Range)y * (Double)x, RealType.Instance, this);
            register.Division((x, y) => (Range)x / (Double)y, this, RealType.Instance);
        }

        public void RegisterOperators(IAddUnary register)
        {
        }

        public void RegisterOperators(IAddLogic register)
        {
        }
    }
}
