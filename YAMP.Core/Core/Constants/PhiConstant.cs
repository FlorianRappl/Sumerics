using System;
using YAMP.Attributes;

namespace YAMP.Core.Constants
{
	/// <summary>
	/// Gets the value of the golden ratio.
	/// </summary>
    [Description("The golden ratio: two quantities are in the golden ratio if the ratio of the sum of the quantities to the larger quantity is equal to the ratio of the larger quantity to the smaller one.")]
    [Link("http://en.wikipedia.org/wiki/Golden_ratio")]
	class PhiConstant : YConstant
	{
        const double phi = 1.61803398874989484820458683436563811;

		public override object Value
		{
			get { return phi; }
		}
	}
}
