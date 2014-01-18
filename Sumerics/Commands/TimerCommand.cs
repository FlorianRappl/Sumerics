using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics
{
	class TimerCommand : YCommand
	{
		public TimerCommand() : base(0, 1)
		{
		}

		public string Invocation()
		{
			return "timer()";
		}

		public string Invocation(string arg)
		{
			int pc = 0;

			if (!int.TryParse(arg, out pc))
			{
				switch(arg)
				{
					case "reset":
						pc = -1;
						break;
					case "stop":
						pc = 0;
						break;
					case "start":
						pc = 1;
						break;
					default:
						return Invocation();
				}
			}

			return "timer(" + pc + ")";
		}
	}
}
