using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumerics
{
	class DateCommand : YCommand
	{
		public DateCommand() : base(0, 1)
		{
		}

		public string Invocation()
		{
			return "date()";
		}

		public string Invocation(string arg)
		{
			return "date(" + arg + ")";
		}
	}
}
