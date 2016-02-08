namespace Sumerics.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class YCommand
	{
		#region Fields

        readonly Dictionary<Int32, Func<Object, Object[], Object>> _invocations;
		readonly Int32 _minArguments;
        readonly Int32 _maxArguments;
        readonly String _name;

		#endregion

		#region ctor

        public YCommand(Int32 minArguments, Int32 maxArguments)
        {
            _invocations = new Dictionary<Int32, Func<Object, Object[], Object>>();
            _minArguments = minArguments;
            _maxArguments = maxArguments;
            var type = GetType();
            _name = type.Name.Replace("Command", String.Empty);
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (method.Name.Equals("Invocation"))
                {
                    var parameters = method.GetParameters();
                    
                    //check if there is a params modified parameter
                    if (maxArguments == Int32.MaxValue && 
                        parameters.Length > 0 && 
                        Attribute.IsDefined(parameters[parameters.Length - 1], typeof(ParamArrayAttribute)))
                    {
                        _invocations.Add(-parameters.Length, method.Invoke);
                    }
                    else
                    {
                        _invocations.Add(parameters.Length, method.Invoke);
                    }
                }
            }
		}

        public YCommand(Int32 minArguments)
            : this(minArguments, Int32.MaxValue)
        {
		}

		#endregion

		#region Properties

		public String Name
		{
            get { return _name; }
		}

        public Int32 MinArguments
        {
            get { return _minArguments; }
        }

        public Int32 MaxArguments
        {
            get { return _maxArguments; }
        }

		#endregion

		#region Execute

        public Boolean CanExecute(Int32 parameters)
        {
            return _invocations.ContainsKey(parameters);
        }

		public String Execute(String[] values)
        {
            var key = values.Length;

            if (_invocations.ContainsKey(key))
            {
                var func = _invocations[key];
                return func(this, values) as String;
            }
            else if (_invocations.Any(item => item.Key < 0))
            {
                //check if there is a params modified parameter
                var invocation = _invocations.First(item => item.Key < 0);
                var count = -invocation.Key - 1;
                var func = invocation.Value;
                var arguments = new Object[count + 1];
                var parameters = new String[key - count];

                for (var i = 0; i < count; i++)
                {
                    arguments[i] = values[i];
                }

                for (var i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = values[i + count];
                }

                arguments[count] = parameters;
                return func(this, arguments) as String;
            }

            return null;
		}

		#endregion
    }
}
