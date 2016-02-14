namespace FastColoredTextBoxNS
{
    using System;
    using System.Drawing;

    class CollapseFoldingMarker : VisualMarker
    {
        public readonly Int32 iLine;

        public CollapseFoldingMarker(Int32 iLine, Rectangle rectangle)
            : base(rectangle)
        {
            this.iLine = iLine;
        }

        public override void Draw(Graphics gr, Pen pen)
        {
            gr.DrawImage(Images.expanded, rectangle);
            gr.DrawEllipse(pen, rectangle);
        }
    }
}
