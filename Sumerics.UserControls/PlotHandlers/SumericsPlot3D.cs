using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using YAMP;
using WPFChart3D;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Sumerics.Controls
{
	abstract class SumericsPlot3D : SumericsPlot
    {
        #region Members

        XYZPlotValue _plot;
        protected Plot3D control;

        #endregion

        #region ctor

        public SumericsPlot3D(XYZPlotValue plot)
            : base(plot)
        {
            _plot = plot;
            control = new Plot3D();
        }

        #endregion

        #region Properties

        public override FrameworkElement Content
        {
            get { return control; }
        }

        public override bool IsGridEnabled
        {
            get
            {
                return true;
            } 
        }

        #endregion

        #region Methods

        public override void RenderToCanvas(Canvas canvas)
        {
            var width = canvas.ActualWidth;
            var height = canvas.ActualHeight;
            var bmp = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);
            var dv = new DrawingVisual();
            var vb = new VisualBrush(control.Viewport);

            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size
                {
                    Height = height,
                    Width = width
                }));
            }

            bmp.Render(dv);
            var img = new Image();
            img.Source = bmp;
            img.Width = width;
            img.Height = height;
            canvas.Children.Add(img);
        }

        public override void CenterPlot()
        {
            control.ResetTransformation();
        }

        public override void ExportPlot(string fileName, int width, int height)
        {
            using (var s = File.Create(fileName))
            {
                var bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
                var dv = new DrawingVisual();
                var vb = new VisualBrush(control.Viewport);

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
                encoder.Save(s);
            }
        }

        #endregion

        #region Empty by design

        public override void ToggleGrid()
        {
            control.ShowAxis = !control.ShowAxis;
        }

        public override void AsPreview()
        {
        }

        #endregion

        #region Helpers

        public static Color[] GenerateColors(ColorPalettes palette, int length)
        {
            var oxycolors = SumericsOxyPlot.GenerateColors(palette, length);
            var c = new Color[length];

            for (var i = 0; i < oxycolors.Length; i++)
                c[i] = Color.FromArgb(oxycolors[i].A, oxycolors[i].R, oxycolors[i].G, oxycolors[i].B);

            return c;
        }

        #endregion
    }
}
