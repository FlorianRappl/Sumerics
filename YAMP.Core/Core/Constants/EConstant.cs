using System;
using YAMP.Attributes;

namespace YAMP.Core.Constants
{
	/// <summary>
	/// Gets the value of euler's number.
	/// </summary>
    [Description("The number e is an important mathematical constant, approximately equal to 2.71828, that is the base of the natural logarithm.")]
    [Link("http://en.wikipedia.org/wiki/E_(mathematical_constant)")]
	class EConstant : YConstant
	{
		public override object Value
		{
			get { return Math.E; }
		}
	}
}
