using System;
using System.Collections.Generic;
using System.Text;

namespace FastColoredTextBoxNS
{
    internal abstract class Command
    {
        internal TextSource ts;
        public abstract void Execute();
    }
}
