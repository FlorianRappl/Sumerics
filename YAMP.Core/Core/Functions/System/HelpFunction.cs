using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Shows detailled help for various topics.")]
	[Kind(KindAttribute.FunctionKind.System)]
    sealed class HelpFunction : YFunction
    {
        [Description("Shows a list of all out-of-the-box provided help topics.")]
        [Example("help()", "Lists all available help topics.")]
        public Nothing Invoke(RunContext ctx)
        {
            WriteLine("Constants:");

            foreach (var entry in ctx.Constants)
                WriteLine("  " + entry.Name);

            WriteLine();

            WriteLine("Functions:");

            foreach (var entry in ctx.Functions)
                WriteLine("  " + entry.Name);

            return Nothing.ToReturn;
        }

        [Description("Shows detailled help for a specific topic.")]
        [Example("help(help)", "You already typed that in!")]
        [Example("help(sin)", "Shows the detailled help for the sin object, which is usually the sine function.")]
        public Nothing Invoke(RunContext ctx, Object topic)
        {
            if (topic is IFunction)
            {
                var f = (IFunction)topic;
                WriteLine(f.Name);
                WriteLine("--------------");
                WriteLine("Category:");
                WriteLine(f.Category);
                WriteLine();
                WriteLine("Description:");
                WriteLine(f.Description);
                WriteLine();
                WriteLine("Hyperlink:");
                WriteLine(f.HyperReference);

                foreach (var usage in f.Overloads)
                {
                    var count = 0;
                    WriteLine();
                    WriteLine("  Usage:");
                    WriteLine("  " + f.Name + "(" + String.Join(", ", usage.ParameterNames) + ")");
                    WriteLine();
                    WriteLine("  Description:");
                    WriteLine("  " + usage.Description);
                    WriteLine();
                    WriteLine("  Arguments:");

                    for (int i = 0; i < usage.Parameters.Length; i++)
                        WriteLine("  +" + Dynamic.FindType(usage.Parameters[i]).Name);

                    WriteLine();
                    WriteLine("  Returns:");
                    WriteLine("  " + Dynamic.FindType(usage.Return).Name);

                    foreach (var example in usage.Examples)
                    {
                        count++;
                        WriteLine();
                        WriteLine("  " + count + ". Example:");
                        WriteLine("     Description: " + example.Description);
                        WriteLine("     Source code: " + example.CodeSnippet);
                    }
                }
            }
            else if (topic is IConstant)
            {
                var c = (IConstant)topic;
                WriteLine(c.Name);
                WriteLine("--------------");
                WriteLine("Value:");
                WriteLine(c.Value.ToString());
                WriteLine();
                WriteLine("Description:");
                WriteLine(c.Description);
                WriteLine();
                WriteLine("Hyperlink:");
                WriteLine(c.HyperReference);
            }
            else
                WriteLine("No help available.");

            //TODO
            return Nothing.ToReturn;
        }

        public Nothing Invoke(RunContext ctx, String topic)
        {
            object obj = null;

            if ((obj = ctx.GetConstant(topic)) != null)
                Invoke(ctx, obj);
            else if ((obj = ctx.GetFunction(topic)) != null)
                Invoke(ctx, obj);
            else if ((obj = ctx.GetVariable(topic)) != null)
                Invoke(ctx, obj);
            else
                WriteLine("Nothing found for the given topic " + topic + ".");

            return Nothing.ToReturn;
        }

        void WriteLine()
        {
            //TODO
        }

        void WriteLine(string msg)
        {
            //TODO
        }
    }
}
