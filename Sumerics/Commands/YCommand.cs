namespace Sumerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    abstract class YCommand
	{
		#region Fields

		Int32 _minArguments;
        Int32 _maxArguments;
        Dictionary<Int32, Func<Object, Object[], Object>> invocations;

		#endregion

		#region ctor

		public YCommand(int minArguments, int maxArguments)
        {
            invocations = new Dictionary<int, Func<object, object[], object>>();
            _minArguments = minArguments;
            _maxArguments = maxArguments;
            var refl = GetType();
            Name = refl.Name.Replace("Command", string.Empty);
            var methods = refl.GetMethods();

            foreach (var method in methods)
            {
                if (method.Name.Equals("Invocation"))
                {
                    var parameters = method.GetParameters();
                    
                    //check if there is a params modified parameter
                    if (maxArguments == int.MaxValue && parameters.Length > 0 && Attribute.IsDefined(parameters[parameters.Length - 1], typeof(ParamArrayAttribute)))
                    {
                        invocations.Add(-method.GetParameters().Length, method.Invoke);
                    }
                    else
                    {
                        invocations.Add(method.GetParameters().Length, method.Invoke);
                    }
                }
            }
		}

        public YCommand(int minArguments) : this(minArguments, int.MaxValue)
        {
		}

		#endregion

		#region Factory

		static Dictionary<string, YCommand> commands = new Dictionary<string, YCommand>();

        public static void RegisterCommands()
        {
            var lib = Assembly.GetExecutingAssembly();
            var types = lib.GetTypes();

            foreach (var type in types)
            {
                if (type.IsAbstract)
                    continue;

                if (type.IsSubclassOf(typeof(YCommand)))
                {
                    var cmd = type.GetConstructor(Type.EmptyTypes).Invoke(null) as YCommand;
                    commands.Add(cmd.Name.ToLower(), cmd);
                }
            }
		}

		public static bool HasCommand(string command)
		{
			return commands.ContainsKey(command);
		}

		public static bool HasOverload(string command, int parameters)
		{
			return commands[command].invocations.ContainsKey(parameters);
		}

        public static string TryCommand(string input)
        {
            input = input ?? string.Empty;
            var token = input;
            var arguments = string.Empty;

            while (input.Length > 0 && input[0] == ' ')
                input = input.Remove(0, 1);

            var index = input.IndexOf(' ');

            if (index > 0)
            {
                token = input.Substring(0, index).ToLower();

                if(index < input.Length - 1)
                    arguments = input.Substring(index + 1);
            }

            if (commands.ContainsKey(token))
            {
                var args = new List<string>();
                var current = new StringBuilder();
                var stringMode = false;

                for (var i = 0; i < arguments.Length; i++)
                {
                    if (arguments[i] == '"')
                        stringMode = !stringMode;
                    else if (arguments[i] == ' ' && !stringMode)
                    {
                        args.Add(current.ToString());
                        current.Clear();
                        continue;
                    }

                    current.Append(arguments[i]);
                }

                if(current.Length > 0)
                    args.Add(current.ToString());

                var cmd = commands[token];

                if (cmd._minArguments > args.Count)
                    return null;
                else if (cmd._maxArguments < args.Count)
                    return null;

                return cmd.Execute(args.ToArray());
            }

            return null;
		}

		#endregion

		#region Properties

		public string Name
		{
			get;
			private set;
		}

		#endregion

		#region Execute

		public string Execute(string[] arguments)
        {
            var key = arguments.Length;

            if (invocations.ContainsKey(key))
            {
                var func = invocations[key];
                return func(this, arguments) as string;
            }
            else if (invocations.Any(item => item.Key < 0))
            {
                //check if there is a params modified parameter
                var invocation = invocations.First(item => item.Key < 0);
                var count = -invocation.Key - 1;
                var func = invocation.Value;
                var args = new object[count + 1];
                var @params = new string[key - count];

                for (int i = 0; i < count; i++)
                    args[i] = arguments[i];

                for (int i = 0; i < @params.Length; i++)
                    @params[i] = arguments[i + count];

                args[count] = @params;
                return func(this, args) as string;
            }

            return null;
		}

		#endregion
    }
}
