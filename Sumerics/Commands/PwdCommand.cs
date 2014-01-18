using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics
{
	class PwdCommand : YCommand
	{
		public PwdCommand() : base(0, 0)
		{
		}

		public string Invocation()
		{
			return "pwd()";
		}
	}
}
