/*
    Copyright (c) 2012-2013, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using YAMP.Core;

namespace YAMP.Physics
{
    public sealed struct Quantity
    {
        #region Members

        String _unit;
        Double _value;

        #endregion

        #region ctor

        public Quantity(string unit)
            : this(0.0, unit)
        {
        }

        public Quantity(double value)
            : this(value, string.Empty)
        {
        }

        public Quantity(double value, string unit)
        {
            _value = value;
            _unit = unit;
        }

        public Quantity(Quantity value)
            : this(value._value, value._unit)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unit.
        /// </summary>
        public String Unit
        {
            get { return _unit; }
        }

        /// <summary>
        /// Gets the Value.
        /// </summary>
        public Double Value
        {
            get { return _value; }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Transforms the instance into a binary representation.
        /// </summary>
        /// <returns>The binary representation.</returns>
        public override byte[] Serialize()
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(Re));
            bytes.AddRange(BitConverter.GetBytes(Im));
            bytes.AddRange(BitConverter.GetBytes(_unit.Length));

            for (int i = 0; i < _unit.Length; i++)
                bytes.AddRange(BitConverter.GetBytes(_unit[i]));

            return bytes.ToArray();
        }

        /// <summary>
        /// Transforms a binary representation into a new instance.
        /// </summary>
        /// <param name="content">The binary data.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(byte[] content)
        {
            var unit = new Quantity();
            unit.Re = BitConverter.ToDouble(content, 0);
            unit.Im = BitConverter.ToDouble(content, 8);
            var str = new char[BitConverter.ToInt32(content, 16)];

            for (int i = 0; i < str.Length; i++)
                str[i] = BitConverter.ToChar(content, 20 + 2 * i);

            unit._unit = new string(str);
            return unit;
        }

        #endregion

        #region Methods

        public override void Clear()
        {
            _unit = string.Empty;
            base.Clear();
        }

        public override ScalarValue Clone()
        {
 	         return new Quantity(Re, Unit);
        }

        public override string ToString(ParseContext context)
        {
            return base.ToString(context) + " " + _unit;
        }

        #endregion

        #region Register Operators

        protected override void RegisterOperators()
        {
            RegisterPlus(typeof(Quantity), typeof(Quantity), AddUU);
            RegisterMinus(typeof(Quantity), typeof(Quantity), SubUU);

            RegisterMultiply(typeof(Quantity), typeof(Quantity), MulUU);
            RegisterMultiply(typeof(ScalarValue), typeof(Quantity), MulSU);
            RegisterMultiply(typeof(Quantity), typeof(ScalarValue), MulUS);

            RegisterDivide(typeof(Quantity), typeof(Quantity), DivUU);
            RegisterDivide(typeof(ScalarValue), typeof(Quantity), DivSU);
            RegisterDivide(typeof(Quantity), typeof(ScalarValue), DivUS);

            RegisterPower(typeof(Quantity), typeof(ScalarValue), PowUS);
        }

        public static Quantity AddUU(Value a, Value b)
        {
            var left = (Quantity)a;
            var right = (Quantity)b;
            var target = ConvertFunction.Convert(right, left.Unit);
            return new Quantity(left + target, left.Unit);
        }

        public static Quantity SubUU(Value a, Value b)
        {
            var left = (Quantity)a;
            var right = (Quantity)b;
            var target = ConvertFunction.Convert(right, left.Unit);
            return new Quantity(left - target, left.Unit);
        }

        public static Quantity MulUU(Value a, Value b)
        {
            var left = (Quantity)a;
            var right = (Quantity)b;
            var unit = new CombinedUnit(left.Unit).Multiply(right.Unit).Simplify();
            return new Quantity(unit.Factor * left * right, unit.Unpack());
        }

        public static Quantity DivUU(Value a, Value b)
        {
            var left = (Quantity)a;
            var right = (Quantity)b;
            var unit = new CombinedUnit(left.Unit).Divide(right.Unit).Simplify();
            return new Quantity(unit.Factor * left / right, unit.Unpack());
        }

        public static Quantity MulSU(Value a, Value b)
        {
            var left = (ScalarValue)a;
            var right = (Quantity)b;
            return new Quantity(left * right, right.Unit);
        }

        public static Quantity DivSU(Value a, Value b)
        {
            var left = (ScalarValue)a;
            var right = (Quantity)b;
            return new Quantity(left / right, right.Unit);
        }

        public static Quantity MulUS(Value a, Value b)
        {
            var left = (Quantity)a;
            var right = (ScalarValue)b;
            return new Quantity(left * right, left.Unit);
        }

        public static Quantity DivUS(Value a, Value b)
        {
            var left = (Quantity)a;
            var right = (ScalarValue)b;
            return new Quantity(left / right, left.Unit);
        }

        public static Quantity PowUS(Value a, Value b)
        {
            var left = (Quantity)a;
            var right = (ScalarValue)b;
            return new Quantity(left.Pow(right), new CombinedUnit(left._unit).Raise(right.Re).Unpack());
        }

        #endregion
    }
}
