namespace Sumerics.MathInput
{
    using Sumerics.MathInput.Parser;
    using System;

    sealed class MathInputService : IMathInput
    {
        public String ConvertToYamp(String markup)
        {
            var parser = new MathMarkupParser(markup);
            return parser.Execute();
        }
    }
}
