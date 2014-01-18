using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("Returns the version of the YAMP parser engine.")]
    [Kind(KindAttribute.FunctionKind.System)]
    sealed class VerFunction : YFunction
    {
        [Description("Gets a string containing the current version of the running YAMP engine.")]
        [Example("ver()", "Gets a string containing the version of YAMP.")]
        public String Invoke(RunContext ctx)
        {
            return ctx.GetPlugin("YAMP.Core").Version;
        }
    }
}
