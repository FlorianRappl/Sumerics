namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Xml.Linq;

    sealed class SqrtParseElement : IParseElement
    {
        public Boolean IsMatched(XElement node)
        {
            return node.Name.LocalName == "msqrt";
        }

        public void Parse(MathMarkupParser parser, XElement node)
        {
            parser.Query.Append("sqrt(");
            parser.Parse(node.Elements());
            parser.Query.Append(")");
        }
    }
}
