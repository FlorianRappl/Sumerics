using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics
{
	class VerCommand : YCommand
	{
		public VerCommand() : base(0, 0)
		{
		}

		public string Invocation()
		{
			return "ver()";
		}
	}
}
