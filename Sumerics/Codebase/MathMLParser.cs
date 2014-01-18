using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sumerics
{
    public class MathMLParser
    {
        #region Members

        XDocument xdoc;
        StringBuilder query;

        #endregion

        #region ctor

        public static string Parse(string xml)
        {
            var parser = new MathMLParser(xml);
            return parser.Execute();
        }

        private MathMLParser(string xml)
        {
            query = new StringBuilder();
            xdoc = XDocument.Parse(xml);
        }

        #endregion

        #region Parse

        public string Execute()
        {
            Parse(xdoc.Root.Elements());
            return query.ToString();
        }

        private void Parse(IEnumerable<XElement> nodes)
        {
            foreach (var node in nodes)
            {
                Parse(node);
            }
        }

        private void Parse(IEnumerable<XElement> nodes, string separator)
        {
            var i = 0;

            foreach (var node in nodes)
            {
                if (i > 0)
                    query.Append(separator);

                Parse(node);
                i++;
            }
        }

        private void Parse(XElement node)
        {
            switch (node.Name.LocalName)
            {
                case "mtable":
                    query.Append("[");
                    Parse(node.Elements(), ";");
                    query.Append("]");
                    break;
                case "mtr":
                    Parse(node.Elements(), ",");
                    break;
                case "mtd":
                    Parse(node.Elements());
                    break;
                case "mfrac":
                    Parse(node.FirstNode as XElement);
                    query.Append("/");
                    Parse(node.LastNode as XElement);
                    break;
                case "mn":
                    goto case "mo";
                case "mi":
                    goto case "mo";
                case "mo":
                    if (StandardValue(node.Value))
                        query.Append(node.Value);
                    else
                        query.Append(SpecialValue(node.Value));
                    break;
                case "mrow":
                    query.Append("(");
                    Parse(node.Elements());
                    query.Append(")");
                    break;
                case "mfenced":
                    query.Append(node.Attribute("open").Value);
                    Parse(node.Elements());
                    query.Append(node.Attribute("close").Value);
                    break;
                case "msqrt":
                    query.Append("sqrt(");
                    Parse(node.Elements());
                    query.Append(")");
                    break;
                case "msup":
                    Parse(node.FirstNode as XElement);
                    string last = (node.LastNode as XElement).Value;
                    if (last == "†" || last == "'")
                    {
                        query.Append("'");
                    }
                    else if (last == "⊤" || last == "+")
                    {
                        query.Append(".'");
                    }
                    else
                    {
                        query.Append("^");
                        Parse(node.LastNode as XElement);
                    }
                    break;
                default:
                    Parse(node.Elements());
                    break;
            }
        }

        bool StandardValue(string value)
        {
            if (value.Length > 1)
                return true;
            if (value[0] < 256)
                return true;
            return false;
        }

        string SpecialValue(string value)
        {
            switch (value)
            {
                case "∑":
                    return "sum";
                //case "∏":
                //    return "product";
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
                    goto case "⇒";
                case "⇒":
                    return "=>";
                default:
                    return String.Empty;
            }
        }

        #endregion

    }
}
