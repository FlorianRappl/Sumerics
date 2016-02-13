namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Linq;

    public sealed class MathMarkupParser
    {
        #region Fields

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

        #region Parse

        public String Execute()
        {
            Parse(_xdoc.Root.Elements());
            return _query.ToString();
        }

        void Parse(IEnumerable<XElement> nodes)
        {
            foreach (var node in nodes)
            {
                Parse(node);
            }
        }

        void Parse(IEnumerable<XElement> nodes, String separator)
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

        private void Parse(XElement node)
        {
            switch (node.Name.LocalName)
            {
                case "mtable":
                    _query.Append("[");
                    Parse(node.Elements(), ";");
                    _query.Append("]");
                    break;
                case "mtr":
                    Parse(node.Elements(), ",");
                    break;
                case "mtd":
                    Parse(node.Elements());
                    break;
                case "mfrac":
                    Parse(node.FirstNode as XElement);
                    _query.Append("/");
                    Parse(node.LastNode as XElement);
                    break;
                case "mn":
                case "mi":
                case "mo":
                    if (StandardValue(node.Value))
                    {
                        _query.Append(node.Value);
                    }
                    else
                    {
                        _query.Append(SpecialValue(node.Value));
                    }
                    break;
                case "mrow":
                    _query.Append("(");
                    Parse(node.Elements());
                    _query.Append(")");
                    break;
                case "mfenced":
                    _query.Append(node.Attribute("open").Value);
                    Parse(node.Elements());
                    _query.Append(node.Attribute("close").Value);
                    break;
                case "msqrt":
                    _query.Append("sqrt(");
                    Parse(node.Elements());
                    _query.Append(")");
                    break;
                case "msup":
                    Parse(node.FirstNode as XElement);
                    var last = (node.LastNode as XElement).Value;

                    if (last == "†" || last == "'")
                    {
                        _query.Append("'");
                    }
                    else if (last == "⊤" || last == "+")
                    {
                        _query.Append(".'");
                    }
                    else
                    {
                        _query.Append("^");
                        Parse(node.LastNode as XElement);
                    }

                    break;
                default:
                    Parse(node.Elements());
                    break;
            }
        }

        Boolean StandardValue(String value)
        {
            return value.Length > 1 || value[0] < 256;
        }

        String SpecialValue(String value)
        {
            switch (value)
            {
                case "∑":
                    return "sum";
                case "∏":
                    return "product";
                case "π":
                    return "pi";
                case "ⅇ":
                    return "e";
                case "α":
                    return "alpha";
                case "β":
                    return "beta";
                case "γ":
                    return "gamma";
                case "δ":
                    return "delta";
                case "ε":
                    return "epsilon";
                case "ζ":
                    return "zeta";
                case "η":
                    return "eta";
                case "θ":
                    return "theta";
                case "λ":
                    return "lambda";
                case "μ":
                    return "mu";
                case "ν":
                    return "nu";
                case "ξ":
                    return "xi";
                case "ρ":
                    return "rho";
                case "σ":
                    return "sigma";
                case "τ":
                    return "tau";
                case "φ":
                    return "phi";
                case "χ":
                    return "chi";
                case "ψ":
                    return "psi";
                case "ω":
                    return "omega";
                case "Γ":
                    return "Gamma";
                case "Δ":
                    return "Delta";
                case "Θ":
                    return "Theta";
                case "Λ":
                    return "Lambda";
                case "Ξ":
                    return "Xi";
                case "Π":
                    return "Pi";
                case "Σ":
                    return "Sigma";
                case "Φ":
                    return "Phi";
                case "Ψ":
                    return "Psi";
                case "Ω":
                    return "Omega";
                case "ϕ":
                    return "phi";
                case "ⅈ":
                    return "I";
                case "∕":
                    return "/";
                case "⋅":
                    return "*";
                case "→":
                case "⇒":
                    return "=>";
                default:
                    return String.Empty;
            }
        }

        #endregion
    }
}
