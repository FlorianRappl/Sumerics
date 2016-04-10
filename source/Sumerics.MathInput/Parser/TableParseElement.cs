namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Xml.Linq;

    sealed class TableParseElement : IParseElement
    {
        public Boolean IsMatched(XElement node)
        {
            return node.Name.LocalName == "mtable";
        }

        public void Parse(MathMarkupParser parser, XElement node)
        {
            parser.Query.Append("[");
            parser.Parse(node.Elements(), ";");
            parser.Query.Append("]");
        }
    }
}
