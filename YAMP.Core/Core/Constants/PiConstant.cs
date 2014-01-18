using System;
using YAMP.Attributes;

namespace YAMP.Core.Constants
{
	/// <summary>
	/// Gets the value of Pi.
	/// </summary>
    [Description("The mathematical constant Pi is the ratio of a circle's circumference to its diameter.")]
    [Link("http://en.wikipedia.org/wiki/Pi")]
	class PiConstant : YConstant
	{
		public override object Value
		{
			get { return Math.PI; }
		}
	}
}
