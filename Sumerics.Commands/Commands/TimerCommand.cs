namespace Sumerics.Commands
{
    using System;

	sealed class TimerCommand : BaseCommand
	{
		public TimerCommand() :
            base(0, 1)
		{
		}

		public String Invocation()
		{
			return "timer()";
		}

		public String Invocation(String arg)
		{
			var pc = 0;

			if (!Int32.TryParse(arg, out pc))
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
