using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FastColoredTextBoxNS
{
    public class VisualMarker
    {
        public readonly Rectangle rectangle;

        public VisualMarker(Rectangle rectangle)
        {
            this.rectangle = rectangle;
        }

        public virtual void Draw(Graphics gr, Pen pen)
        {
        }

        public virtual Cursor Cursor
        {
            get { return Cursors.Hand; }
        }
    }

    class CollapseFoldingMarker: VisualMarker
    {
        public readonly int iLine;

        public CollapseFoldingMarker(int iLine, Rectangle rectangle)
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

    class ExpandFoldingMarker : VisualMarker
    {
        public readonly int iLine;

        public ExpandFoldingMarker(int iLine, Rectangle rectangle)
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

    public class FoldedAreaMarker : VisualMarker
    {
        public readonly int iLine;

        public FoldedAreaMarker(int iLine, Rectangle rectangle)
            : base(rectangle)
        {
            this.iLine = iLine;
        }

        public override void Draw(Graphics gr, Pen pen)
        {
            gr.DrawRectangle(pen, rectangle);
        }
    }

    public class StyleVisualMarker : VisualMarker
    {
        public Style Style{get;private set;}

        public StyleVisualMarker(Rectangle rectangle, Style style)
            : base(rectangle)
        {
            this.Style = style;
        }
    }

    public class VisualMarkerEventArgs : MouseEventArgs
    {
        public Style Style { get; private set; }
        public StyleVisualMarker Marker { get; private set; }

        public VisualMarkerEventArgs(Style style, StyleVisualMarker marker, MouseEventArgs args)
            : base(args.Button, args.Clicks, args.X, args.Y, args.Delta)
        {
            this.Style = style;
            this.Marker = marker;
        }
    }
}
