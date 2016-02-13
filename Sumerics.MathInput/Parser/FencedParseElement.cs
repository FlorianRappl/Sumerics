namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Xml.Linq;

    sealed class FencedParseElement : IParseElement
    {
        public Boolean IsMatched(XElement node)
        {
            return node.Name.LocalName == "mfenced";
        }

        public void Parse(MathMarkupParser parser, XElement node)
        {
            parser.Query.Append(node.Attribute("open").Value);
            parser.Parse(node.Elements());
            parser.Query.Append(node.Attribute("close").Value);
        }
    }
}
