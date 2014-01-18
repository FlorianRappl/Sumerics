using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YAMP.Core;
using YAMP.Parser;

namespace YAMP.Expressions
{
    sealed class EmptyExpression : Expression
    {
        static readonly EmptyExpression _instance = new EmptyExpression();

        private EmptyExpression()
            : base(Token.End(1, 1))
        {
        }

        public override Dynamic Evaluate(RunContext ctx)
        {
            return new Dynamic(Nothing.ToReturn);
        }

        public override string ToCode()
        {
            return string.Empty;
        }

        public static EmptyExpression Instance
        {
            get { return _instance; }
        }
    }
}
