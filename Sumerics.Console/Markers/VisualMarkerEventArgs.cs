namespace FastColoredTextBoxNS
{
    using System.Windows.Forms;

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
