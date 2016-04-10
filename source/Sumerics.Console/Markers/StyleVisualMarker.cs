namespace FastColoredTextBoxNS
{
    using System.Drawing;

    public class StyleVisualMarker : VisualMarker
    {
        public Style Style { get; private set; }

        public StyleVisualMarker(Rectangle rectangle, Style style)
            : base(rectangle)
        {
            this.Style = style;
        }
    }
}
