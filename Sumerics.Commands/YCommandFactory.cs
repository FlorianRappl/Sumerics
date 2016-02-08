namespace Sumerics.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class YCommandFactory
    {
        #region Fields

        readonly Dictionary<String, YCommand> _commands;
        readonly IContainer _container;

        #endregion

        #region ctor

        public YCommandFactory(IContainer container)
        {
            _commands = new Dictionary<String, YCommand>();
            _container = container;
        }

        #endregion

        #region Methods

        public void RegisterCommands()
        {
            var lib = Assembly.GetExecutingAssembly();
            RegisterCommands(lib);
        }

        public void RegisterCommands(Assembly library)
        {
            foreach (var type in library.GetTypes())
            {
                if (!type.IsAbstract)
                {
                    if (type.IsSubclassOf(typeof(YCommand)))
                    {
                        var command = _container.Create(type) as YCommand;

                        if (command != null)
                        {
                            RegisterCommand(command);
                        }
                    }
                }
            }
        }

        public void RegisterCommand(YCommand command)
        {
            _commands.Add(command.Name.ToLower(), command);
        }

        public Boolean HasCommand(String command)
        {
            return _commands.ContainsKey(command);
        }

        public Boolean HasOverload(String command, Int32 parameters)
        {
            return _commands[command].CanExecute(parameters);
        }

        public String TryCommand(String input)
        {
            input = input ?? String.Empty;
            var token = input;
            var arguments = String.Empty;

            while (input.Length > 0 && input[0] == ' ')
            {
                input = input.Remove(0, 1);
            }

            var index = input.IndexOf(' ');

            if (index > 0)
            {
                token = input.Substring(0, index).ToLower();

                if (index < input.Length - 1)
                {
                    arguments = input.Substring(index + 1);
                }
            }

            if (_commands.ContainsKey(token))
            {
                var args = new List<String>();
                var current = new StringBuilder();
                var stringMode = false;

                for (var i = 0; i < arguments.Length; i++)
                {
                    if (arguments[i] == '"')
                    {
                        stringMode = !stringMode;
                    }
                    else if (arguments[i] == ' ' && !stringMode)
                    {
                        args.Add(current.ToString());
                        current.Clear();
                        continue;
                    }

                    current.Append(arguments[i]);
                }

                if (current.Length > 0)
                {
                    args.Add(current.ToString());
                }

                var cmd = _commands[token];

                if (cmd.MinArguments > args.Count)
                {
                    return null;
                }
                else if (cmd.MaxArguments < args.Count)
                {
                    return null;
                }

                return cmd.Execute(args.ToArray());
            }

            return null;
        }

        #endregion
    }
}
