namespace FastColoredTextBoxNS
{
    using System;
    using System.Drawing;

    class ExpandFoldingMarker : VisualMarker
    {
        public readonly Int32 iLine;

        public ExpandFoldingMarker(Int32 iLine, Rectangle rectangle)
            : base(rectangle)
        {
            this.iLine = iLine;
        }

        public override void Draw(Graphics gr, Pen pen)
        {
            gr.DrawImage(Images.collapsed, rectangle);
            gr.DrawEllipse(pen, rectangle);
        }
    }
}
