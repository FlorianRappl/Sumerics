namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Xml.Linq;

    interface IParseElement
    {
        Boolean IsMatched(XElement node);

        void Parse(MathMarkupParser parser, XElement node);
    }
}
