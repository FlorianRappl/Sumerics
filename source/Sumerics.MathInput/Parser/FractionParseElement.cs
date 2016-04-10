namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Xml.Linq;

    sealed class FractionParseElement : IParseElement
    {
        public Boolean IsMatched(XElement node)
        {
            return node.Name.LocalName == "mfrac";
        }

        public void Parse(MathMarkupParser parser, XElement node)
        {
            parser.Parse(node.FirstNode as XElement);
            parser.Query.Append("/");
            parser.Parse(node.LastNode as XElement);
        }
    }
}
