using System;
using System.Collections.Generic;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Deletes variables from memory.")]
	[Kind(KindAttribute.FunctionKind.System)]
    sealed class ClearFunction : YFunction
    {
        [Description("Clears all variables.")]
        [Example("clear()")]
        public Nothing Invoke(RunContext ctx)
        {
            var names = new List<String>();

            foreach (var variable in ctx.Variables)
                names.Add(variable.Key);

            return Invoke(ctx, names.ToArray());
        }

        [Description("Clears the specified variable given with the name as string.")]
        [Example("clear(\"x\")", "Deletes the variable x.")]
        public Nothing Invoke(RunContext ctx, String variable)
        {
            return Invoke(ctx, new String[] { variable });
        }

        [Description("Clears the specified variables given with their names as strings.")]
        [Example("clear([\"x\"])", "Deletes the variable x.")]
        [Example("clear([\"x\", \"y\", \"z\"])", "Deletes the variables x, y and z.")]
        public Nothing Invoke(RunContext ctx, String[] variables)
        {
            var count = 0;

            for (int i = 0; i < variables.Length; i++)
            {
                if (ctx.GetVariable(variables[i]) != null)
                {
                    ctx.SetVariable(variables[i], null);
                    count++;
                }
            }

            return Nothing.ToReturn;
        }
    }
}
