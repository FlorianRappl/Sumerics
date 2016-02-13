namespace Sumerics.MathInput.Parser
{
    using System;
    using System.Xml.Linq;

    sealed class ValueParseElement : IParseElement
    {
        public Boolean IsMatched(XElement node)
        {
            var name = node.Name.LocalName;
            return name == "mn" || name == "mi" || name == "mo";
        }

        public void Parse(MathMarkupParser parser, XElement node)
        {
            if (IsStandardValue(node.Value))
            {
                parser.Query.Append(node.Value);
            }
            else
            {
                var value = SpecialValue(node.Value);
                parser.Query.Append(value);
            }
        }

        static Boolean IsStandardValue(String value)
        {
            return value.Length > 1 || value[0] < 256;
        }

        static String SpecialValue(String value)
        {
            switch (value)
            {
                case "∑": return "sum";
                case "∏": return "product";
                case "π": return "pi";
                case "ⅇ": return "e";
                case "α": return "alpha";
                case "β": return "beta";
                case "γ": return "gamma";
                case "δ": return "delta";
                case "ε": return "epsilon";
                case "ζ": return "zeta";
                case "η": return "eta";
                case "θ": return "theta";
                case "λ": return "lambda";
                case "μ": return "mu";
                case "ν": return "nu";
                case "ξ": return "xi";
                case "ρ": return "rho";
                case "σ": return "sigma";
                case "τ": return "tau";
                case "φ": return "phi";
                case "χ": return "chi";
                case "ψ": return "psi";
                case "ω": return "omega";
                case "Γ": return "Gamma";
                case "Δ": return "Delta";
                case "Θ": return "Theta";
                case "Λ": return "Lambda";
                case "Ξ": return "Xi";
                case "Π": return "Pi";
                case "Σ": return "Sigma";
                case "Φ": return "Phi";
                case "Ψ": return "Psi";
                case "Ω": return "Omega";
                case "ϕ": return "phi";
                case "ⅈ": return "I";
                case "∕": return "/";
                case "⋅": return "*";
                case "→": return "=>";
                case "⇒": return "=>";
                default: return String.Empty;
            }
        }
    }
}
