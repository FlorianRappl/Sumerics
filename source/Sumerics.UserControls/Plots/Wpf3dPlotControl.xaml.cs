namespace Sumerics.Controls.Plots
{
    using WPFChart3D;

    /// <summary>
    /// Interaction logic for Wpf3dPlotControl.xaml
    /// </summary>
    public partial class Wpf3dPlotControl : BasePlotControl
    {
        static readonly Plot3D Dummy = default(Plot3D);

        public Wpf3dPlotControl()
        {
            InitializeComponent();
        }

        /*
        public void ExportAsPng(Stream stream, Int32 width, Int32 height)
        {
            var bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            var dv = new DrawingVisual();
            var vb = new VisualBrush(Plot.Viewport);

            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size
                {
                    Height = height,
                    Width = width
                }));
            }

            bmp.Render(dv);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(stream);
        }
         */
    }
}
