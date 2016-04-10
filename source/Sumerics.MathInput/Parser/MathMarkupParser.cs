namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Linq;

    public sealed class MathMarkupParser
    {
        #region Fields

        static readonly IParseElement[] Elements = new IParseElement[]
        {
            new ValueParseElement(),
            new FractionParseElement(),
            new PowParseElement(),
            new SqrtParseElement(),
            new GroupParseElement(),
            new TableParseElement(),
            new RowParseElement(),
            new ColumnParseElement(),
            new FencedParseElement()
        };

        readonly XDocument _xdoc;
        readonly StringBuilder _query;

        #endregion

        #region ctor

        public MathMarkupParser(String xml)
        {
            _query = new StringBuilder();
            _xdoc = XDocument.Parse(xml);
        }

        #endregion

        #region Properties

        public StringBuilder Query
        {
            get { return _query; }
        }

        #endregion

        #region Parse

        public String Execute()
        {
            Parse(_xdoc.Root.Elements());
            return _query.ToString();
        }

        public void Parse(IEnumerable<XElement> nodes)
        {
            foreach (var node in nodes)
            {
                Parse(node);
            }
        }

        public void Parse(IEnumerable<XElement> nodes, String separator)
        {
            var i = 0;

            foreach (var node in nodes)
            {
                if (i > 0)
                {
                    _query.Append(separator);
                }

                Parse(node);
                i++;
            }
        }

        public void Parse(XElement node)
        {
            foreach (var element in Elements)
            {
                if (element.IsMatched(node))
                {
                    element.Parse(this, node);
                    return;
                }
            }

            Parse(node.Elements());
        }


        #endregion
    }
}
