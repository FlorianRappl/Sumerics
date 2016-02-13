namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Xml.Linq;

    sealed class RowParseElement : IParseElement
    {
        public Boolean IsMatched(XElement node)
        {
            return node.Name.LocalName == "mtr";
        }

        public void Parse(MathMarkupParser parser, XElement node)
        {
            parser.Parse(node.Elements(), ",");
        }
    }
}
