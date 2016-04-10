using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FastColoredTextBoxNS
{
    public class PaintLineEventArgs : PaintEventArgs
    {
        public PaintLineEventArgs(int iLine, Rectangle rect, Graphics gr, Rectangle clipRect)
            : base(gr, clipRect)
        {
            LineIndex = iLine;
            LineRect = rect;
        }

        public int LineIndex { get; set; }
        public Rectangle LineRect { get; set; }
    }
}
