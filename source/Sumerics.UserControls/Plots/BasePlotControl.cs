namespace Sumerics.Controls.Plots
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public abstract class BasePlotControl : UserControl, IPlotSerializer
    {
        public void Print(PrintDialog printDialog, String title)
        {
            var canvas = PlotOnCanvas(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
            printDialog.PrintVisual(canvas, title);
        }

        public void Save(FileStream fs, Int32 width, Int32 height)
        {
            var canvas = PlotOnCanvas(width, height);
            var renderBitmap = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Pbgra32);
            var encoder = new PngBitmapEncoder();
            renderBitmap.Render(canvas);
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(fs);
        }

        Canvas PlotOnCanvas(Double width, Double height)
        {
            var canvas = new Canvas
            {
                Width = width,
                Height = height
            };

            canvas.Measure(new Size(width, height));
            canvas.Arrange(new Rect(0.0, 0.0, width, height));
            RenderToCanvas(canvas);
            canvas.UpdateLayout();
            return canvas;
        }

        protected abstract void RenderToCanvas(Canvas canvas);
    }
}
