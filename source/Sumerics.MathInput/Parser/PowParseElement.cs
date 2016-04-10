namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Xml.Linq;

    sealed class PowParseElement : IParseElement
    {
        public Boolean IsMatched(XElement node)
        {
            return node.Name.LocalName == "msup";
        }

        public void Parse(MathMarkupParser parser, XElement node)
        {
            parser.Parse(node.FirstNode as XElement);
            var last = (node.LastNode as XElement).Value;

            if (last == "†" || last == "'")
            {
                parser.Query.Append("'");
            }
            else if (last == "⊤" || last == "+")
            {
                parser.Query.Append(".'");
            }
            else
            {
                parser.Query.Append("^");
                parser.Parse(node.LastNode as XElement);
            }
        }
    }
}
