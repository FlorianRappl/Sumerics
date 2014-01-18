using System;
using System.Text.RegularExpressions;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Lists the available variables.")]
	[Kind(KindAttribute.FunctionKind.System)]
    sealed class WhoFunction : YFunction
    {
        [Description("Lists all variables from the current workspace.")]
        [Example("who()")]
        public Nothing Invoke(RunContext ctx)
        {
            foreach (var variable in ctx.Variables)
                WriteLine(variable.Key + " : " + variable.Value.Type);

            return Nothing.ToReturn;
        }

        [Description("Lists variables from the current workspace using a filter.")]
        [Example("who(\"a*\")", "Lists all variables, which start with a small 'a'.")]
        [Example("who(\"x?b\")", "Lists all variables, which contain 3 letters, starting with a small x, ending with a small b and any letter in between.")]
        public Nothing Invoke(RunContext ctx, String filter)
        {
            var regex = new Regex("^" + Regex.Escape(filter).Replace("\\*", ".*").Replace("\\?", ".{1}") + "$");
            
            foreach (var variable in ctx.Variables)
            {
                if (regex.IsMatch(variable.Key))
                    WriteLine(variable.Key + " : " + variable.Value.Type);
            }

            return Nothing.ToReturn;
        }

		[Description("Lists variables from the current workspace using a filter.")]
		[Example("who([\"a*\", \"x\"])", "Lists all variables, which start with a small 'a' and the variable x.")]
		[Example("who([\"x?b\", \"a\", \"b\"])", "Lists the variables a and b, as well as all variables, which contain 3 letters, starting with a small x, ending with a small b and any letter in between.")]
        public Nothing Invoke(RunContext ctx, String[] filters)
		{
            for (int i = 0; i < filters.Length; i++)
                Invoke(ctx, filters[i]);

            return Nothing.ToReturn;
		}

        void WriteLine(string msg)
        {
            //TODO
        }
    }
}
