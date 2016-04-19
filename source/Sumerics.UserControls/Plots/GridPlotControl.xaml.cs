namespace Sumerics.Controls.Plots
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for GridPlotControl.xaml
    /// </summary>
    public partial class GridPlotControl : BasePlotControl
    {
        public GridPlotControl()
        {
            InitializeComponent();
        }

        protected override void RenderToCanvas(Canvas canvas)
        {
            var dv = new DrawingVisual();
            var vb = new VisualBrush(Plotter);
            var width = (Int32)canvas.Width;
            var height = (Int32)canvas.Height;
            var renderBitmap = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Pbgra32);
            var encoder = new PngBitmapEncoder();

            using (var dc = dv.RenderOpen())
            {
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size
                {
                    Height = height,
                    Width = width
                }));
            }

            renderBitmap.Render(dv);
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            canvas.Children.Add(new Image { Source = renderBitmap });
        }
    }
}
