namespace FastColoredTextBoxNS
{
    using System;
    using System.Drawing;

    public class FoldedAreaMarker : VisualMarker
    {
        public readonly Int32 iLine;

        public FoldedAreaMarker(Int32 iLine, Rectangle rectangle)
            : base(rectangle)
        {
            this.iLine = iLine;
        }

        public override void Draw(Graphics gr, Pen pen)
        {
            gr.DrawRectangle(pen, rectangle);
        }
    }
}
