/*
	Copyright (c) 2012-2013, Florian Rappl et al.
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
using YAMP.Types;

namespace YAMP
{
    /// <summary>
    /// The whole core of the dynamic type system of
    /// YAMP² is defined in this class.
    /// </summary>
    public sealed class Dynamic
    {
        #region Members

        Object value;
        IType type;

        static readonly Dictionary<IType, Type[]> types;
        static readonly IType objectType;

        #endregion

        #region ctor

        static Dynamic()
        {
            types = new Dictionary<IType, Type[]>();

            objectType = new ObjectType();
            RegisterType(new RealType(), typeof(Single), typeof(Double));
            RegisterType(new IntegerType(), typeof(Int16), typeof(Int32), typeof(Int64));
            RegisterType(new ComplexType(), typeof(Complex));
            RegisterType(new BooleanType(), typeof(Boolean));
            RegisterType(new StringType(), typeof(String));
            RegisterType(new FunctionType());
            RegisterType(new MatrixType(), typeof(Matrix));
            RegisterType(new ExceptionType(), typeof(Exception));
            RegisterType(objectType, typeof(Object));
        }

        /// <summary>
        /// Creates a new dynamic type containing
        /// the given value.
        /// </summary>
        /// <param name="value">The value to contain.</param>
        /// <param name="type">The type to describe.</param>
        internal Dynamic(Object value, IType type)
        {
            this.value = value;
            this.type = type;
        }

        /// <summary>
        /// Creates a new dynamic type with no value.
        /// </summary>
        public Dynamic()
        {
            Value = Nothing.ToReturn;
        }

        /// <summary>
        /// Creates a new dynamic type containing
        /// the given value.
        /// </summary>
        /// <param name="value">The value to contain.</param>
        public Dynamic(Object value)
        {
            Value = value ?? Nothing.ToReturn;
        }

        /// <summary>
        /// Creates a new dynamic type based on the value
        /// of the given dynamic type.
        /// </summary>
        /// <param name="instance">The original instance.</param>
        public Dynamic(Dynamic instance)
        {
            Value = instance.value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contained value.
        /// </summary>
        public Object Value
        {
            get { return value; }
            private set 
            { 
                this.value = value;
                this.type = FindType(value);
            }
        }

        /// <summary>
        /// Gets the type of the current value.
        /// </summary>
        internal IType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Gets if the given value is actually nothing.
        /// </summary>
        public Boolean IsNull
        {
            get { return value == Nothing.ToReturn; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the value to the given type.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <returns>The converted value.</returns>
        public T ConvertTo<T>()
        {
            var type = FindType(typeof(T));

            if (type != null)
                return (T)type.Convert(this);

            return default(T);
        }

        /// <summary>
        /// Tries to use the current value like as a function with the given arguments.
        /// </summary>
        /// <param name="args">The arguments for the function.</param>
        /// <returns>The returned value.</returns>
        public Dynamic Call(Dynamic[] args)
        {
            if (type is ICallable)
                return ((ICallable)type).Invoke(value, args);

            throw new ArgumentException("The given expression cannot be called.");
        }

        #endregion

        #region ToString

        /// <summary>
        /// Gets the string representation of the content.
        /// </summary>
        /// <returns>The string representation.</returns>
        public String Stringify()
        {
            return type.ToString(value);
        }

        /// <summary>
        /// Gets the code representation of the content.
        /// </summary>
        /// <returns>The string representation.</returns>
        public String Codify()
        {
            return type.ToCode(value);
        }

        #endregion

        #region Transporter

        /// <summary>
        /// Registers the specified type with the given definition.
        /// </summary>
        /// <param name="type">The type to register.</param>
        static void RegisterType(IType type, params Type[] mapping)
        {
            if (!types.ContainsKey(type))
                types.Add(type, mapping);
        }

        /// <summary>
        /// Unregisters the given type from the list of types.
        /// </summary>
        /// <param name="type">The type to remove.</param>
        static void UnregisterType(IType type)
        {
            if (types.ContainsKey(type))
                types.Remove(type);
        }

        /// <summary>
        /// Gets the specific transporter for the given instance.
        /// </summary>
        /// <param name="value">The instance that is represented by some transporter.</param>
        /// <returns>The responsible transporter.</returns>
        internal static IType FindType(Object value)
        {
            if (value is DynamicList)
                return new ArrayType(FindType((DynamicList)value));

            foreach (var type in types)
            {
                if (type.Key.IsType(value))
                    return type.Key;
            }

            return objectType;
        }

        /// <summary>
        /// Gets the specific transporter for the given type.
        /// </summary>
        /// <param name="clr">The underlying type of the transporter.</param>
        /// <returns>The responsible transporter.</returns>
        internal static IType FindType(Type clr)
        {
            if (clr.IsArray)
                return new ArrayType(FindType(clr.GetElementType()));

            foreach (var type in types)
            {
                for (int i = 0; i < type.Value.Length; i++)
                {
                    if (type.Value[i] == clr)
                        return type.Key;
                }
            }

            return objectType;
        }

        /// <summary>
        /// Gets the specific transporter for the given dynamic list.
        /// </summary>
        /// <param name="list">The dynamic list.</param>
        /// <returns>The common type of the list.</returns>
        internal static IType FindType(DynamicList list)
        {
            if (list.Count == 0)
                return objectType;

            var top = list[0].type;

            for (int i = 1; i < list.Count; i++)
            {
                if (top == objectType)
                    return top;

                var rel = list[i].type.RelationTo(top);

                switch (rel)
                {
                    case TypeMetric.Derived:
                        top = list[i].type;
                        break;

                    case TypeMetric.None:
                        top = objectType;
                        break;
                }
            }

            return top;
        }

        #endregion

        #region Equality

        /// <summary>
        /// Gets the hashcode (of th value).
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// Compares the wrapped value with the given value.
        /// </summary>
        /// <param name="obj">Another object to compare with. In case
        /// of a Dynamic type the content will be taken.</param>
        /// <returns>The result of the equality comparison.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is Dynamic)
                obj = ((Dynamic)obj).value;

            if (value != null)
                return value.Equals(obj);
            else if (obj != null)
                return obj.Equals(value);

            return true;
        }

        #endregion

        #region Special operators

        /// <summary>
        /// Transposes the value.
        /// </summary>
        /// <returns>The dynamic instance with the transposed value.</returns>
        public Dynamic Transpose()
        {
            if (type is ITransposable)
                Value = ((ITransposable)type).Transpose(value);

            return this;
        }

        /// <summary>
        /// Adjungates the value.
        /// </summary>
        /// <returns>The dynamic instance with the adjungated value.</returns>
        public Dynamic Adjungate()
        {
            if (type is ITransposable)
                Value = ((ITransposable)type).Adjungate(value);

            return this;
        }

        #endregion

        #region Binary Math Operators

        public static Dynamic operator +(Dynamic a, Dynamic b)
        {
            if (a.IsNull)
                return b;
            else if (b.IsNull)
                return a;

            var type = Pick<IAddable>(a, b);

            if (type != null)
                return new Dynamic(type.Add(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The + operator is not supported with the given arguments.");
        }

        public static Dynamic operator -(Dynamic a, Dynamic b)
        {
            if (a.IsNull)
                return b;
            else if (b.IsNull)
                return a;

            var type = Pick<IAddable>(a, b);

            if (type != null)
                return new Dynamic(type.Sub(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The - operator is not supported with the given arguments.");
        }

        public static Dynamic operator *(Dynamic a, Dynamic b)
        {
            if (a.IsNull)
                return b;
            else if (b.IsNull)
                return a;

            var type = Pick<IPowable>(a, b);

            if (type != null)
                return new Dynamic(type.Mul(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The * operator is not supported with the given arguments.");
        }

        public static Dynamic operator /(Dynamic a, Dynamic b)
        {
            if (a.IsNull)
                return b;
            else if (b.IsNull)
                return a;

            var type = Pick<IPowable>(a, b);

            if (type != null)
                return new Dynamic(type.Div(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The / operator is not supported with the given arguments.");
        }

        public static Dynamic operator ^(Dynamic a, Dynamic b)
        {
            if (a.IsNull)
                return b;
            else if (b.IsNull)
                return a;

            var type = Pick<IPowable>(a, b);

            if (type != null)
                return new Dynamic(type.Pow(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The ^ operator is not supported with the given arguments.");
        }

        public static Dynamic operator %(Dynamic a, Dynamic b)
        {
            if (a.IsNull)
                return b;
            else if (b.IsNull)
                return a;

            var type = Pick<IPowable>(a, b);

            if (type != null)
                return new Dynamic(type.Mod(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The % operator is not supported with the given arguments.");
        }

        #endregion

        #region Binary Bool Operators

        public static Boolean operator ==(Dynamic a, Dynamic b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if ((Object)a == null || (Object)b == null)
                return false;

            var type = Pick(a, b) ?? objectType;
            return type.AreEqual(type.Convert(a), type.Convert(b));
        }

        public static Boolean operator !=(Dynamic a, Dynamic b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
                return false;

            // If one is null, but not both, return false.
            if ((Object)a == null || (Object)b == null)
                return true;

            var type = Pick(a, b) ?? objectType;
            return type.AreNotEqual(type.Convert(a), type.Convert(b));
        }

        public static Dynamic operator <(Dynamic a, Dynamic b)
        {
            var type = Pick<IMeasurable>(a, b);

            if (type != null)
                return new Dynamic(type.LessThan(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The < operator is not supported with the given arguments.");
        }

        public static Dynamic operator >(Dynamic a, Dynamic b)
        {
            var type = Pick<IMeasurable>(a, b);

            if (type != null)
                return new Dynamic(type.GreaterThan(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The > operator is not supported with the given arguments.");
        }

        public static Dynamic operator <=(Dynamic a, Dynamic b)
        {
            var type = Pick<IMeasurable>(a, b);

            if (type != null)
                return new Dynamic(type.LessEqual(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The <= operator is not supported with the given arguments.");
        }

        public static Dynamic operator >=(Dynamic a, Dynamic b)
        {
            var type = Pick<IMeasurable>(a, b);

            if (type != null)
                return new Dynamic(type.GreaterEqual(type.Convert(a), type.Convert(b)));

            throw new ArgumentException("The >= operator is not supported with the given arguments.");
        }

        public static Dynamic operator &(Dynamic a, Dynamic b)
        {
            if (a.type.IsTrue(a.value))
                return b;
            
            return a;
        }

        public static Dynamic operator |(Dynamic a, Dynamic b)
        {
            if (a.type.IsTrue(a.value))
                return a;

            return b;
        }

        #endregion

        #region Unary operators

        /// <summary>
        /// The not operator (invert boolean value).
        /// </summary>
        /// <param name="a">The argument to invert.</param>
        /// <returns>A new dynamic type.</returns>
        public static Dynamic operator !(Dynamic a)
        {
            if (a.IsNull) return new Dynamic(true);
            return new Dynamic(!a.type.IsTrue(a.value));
        }

        /// <summary>
        /// The faculty operator.
        /// </summary>
        /// <param name="a">The argument for the faculty operation.</param>
        /// <returns>A new dynamic type.</returns>
        public static Dynamic operator ~(Dynamic a)
        {
            if (a.IsNull)
                return a;
            else if (a.type is IPowable)
                return new Dynamic(((IPowable)a.type).Factorial(a.value));

            throw new ArgumentException(a.type.Name + " types cannot be used with the factorial operation.");
        }

        /// <summary>
        /// The negation operator.
        /// </summary>
        /// <param name="a">The argument for the negation operation.</param>
        /// <returns>A new dynamic type.</returns>
        public static Dynamic operator -(Dynamic a)
        {
            if (a.IsNull)
                return a;
            else if (a.type is IAddable)
                return new Dynamic(((IAddable)a.type).Negate(a.value));

            throw new ArgumentException(a.type.Name + " types cannot be negated.");
        }

        /// <summary>
        /// The neutral operator.
        /// </summary>
        /// <param name="a">The argument for the neutral operation.</param>
        /// <returns>A given dynamic type.</returns>
        public static Dynamic operator +(Dynamic a)
        {
            return a;
        }

        /// <summary>
        /// The converts-to-true operator, i.e. if (d) where d is dynamic
        /// </summary>
        /// <param name="a">The argument for the convertion.</param>
        /// <returns>A boolean.</returns>
        public static Boolean operator true(Dynamic a)
        {
            return (Object)a != null && !a.IsNull && a.type.IsTrue(a.value);
        }

        /// <summary>
        /// The converts-to-false operator, i.e. d && true where d is dynamic
        /// </summary>
        /// <param name="a">The argument for the convertion.</param>
        /// <returns>A boolean.</returns>
        public static Boolean operator false(Dynamic a)
        {
            return (Object)a == null || a.IsNull;
        }

        #endregion

        #region Helpers

        static IType Pick(Dynamic a, Dynamic b)
        {
            if (a.type == b.type)
                return a.type;

            var relA = a.type.RelationTo(b.type);
            var relB = b.Type.RelationTo(a.Type);

            if (relA == TypeMetric.None)
            {
                if (relB == TypeMetric.None)
                    return null;

                return b.type;
            }
            else if (relB == TypeMetric.None)
                return a.type;

            return (Int32)relA < (Int32)relB ? a.type : b.type;
        }

        static T Pick<T>(Dynamic a, Dynamic b)
        {
            if (a.type == b.type)
            {
                if (a.type is T)
                    return (T)a.type;

                return default(T);
            }

            var relA = a.type is T ? a.type.RelationTo(b.type) : TypeMetric.None;
            var relB = b.type is T ? b.Type.RelationTo(a.Type) : TypeMetric.None;

            if (relA == TypeMetric.None)
            {
                if (relB == TypeMetric.None)
                    return default(T);

                return (T)b.type;
            }
            else if (relB == TypeMetric.None)
                return (T)a.type;

            return (Int32)relA < (Int32)relB ? (T)a.type : (T)b.type;
        }

        #endregion
    }
}
