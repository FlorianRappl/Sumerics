using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
	[Description("Gets information about the type of variables.")]
    [Kind(KindAttribute.FunctionKind.System)]
    sealed class TypeFunction : YFunction
    {
        [Description("Requests information about the type for the specified variable.")]
        [Example("type(x)", "Gets the type information of the variable x.")]
        public String Invoke(RunContext ctx, Dynamic instance)
        {
            return instance.Type.Name;
        }
    }
}
