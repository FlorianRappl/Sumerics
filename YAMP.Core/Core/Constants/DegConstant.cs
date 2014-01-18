using System;
using YAMP.Attributes;

namespace YAMP.Core.Constants
{
    /// <summary>
    /// Gets the value of one degree.
    /// </summary>
    [Description("A degree (in full, a degree of arc, arc degree, or arcdegree), usually denoted by ° (the degree symbol), is a measurement of plane angle, representing 1⁄360 of a full rotation; one degree is equivalent to π/180 radians.")]
    [Link("http://en.wikipedia.org/wiki/Degree_(angle)")]
    class DegConstant : YConstant
    {
        const double deg = Math.PI / 180.0;

        public override object Value
        {
            get { return deg; }
        }
    }
}
