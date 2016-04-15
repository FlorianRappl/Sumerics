namespace Sumerics.Controls.Plots
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for Wpf3dPlotControl.xaml
    /// </summary>
    public partial class Wpf3dPlotControl : BasePlotControl
    {
        public Wpf3dPlotControl()
        {
            InitializeComponent();
            /*
            // Surface:

            _control.CreateSurface(plot.Nx, plot.Ny);
            RefreshData();

            // Mesh:

            RefreshData();
            RefreshProperties();*/
        }

        public void RefreshData()
        {
            throw new NotImplementedException();
        }

        public void RefreshProperties()
        {
            throw new NotImplementedException();
        }

        public void RefreshSeries()
        {
            throw new NotImplementedException();
        }

        public void ToggleGrid()
        {
            Plot.ShowAxis = !Plot.ShowAxis;
        }

        public void PreviewMode()
        {
        }

        public void CenterPlot()
        {
            Plot.ResetTransformation();
        }

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
        /*
        // MESH:

        public override void RefreshData()
        {
            _control.SetWireframe(false);
            _isLogx = _plot.IsLogX;
            _isLogy = _plot.IsLogY;
            _isLogz = _plot.IsLogZ;

            for (var i = 0; i < _plot.Count; i++)
            {
                var series = _plot[i];
                _control.WireframeColor = series.Color.ColorFromString();
                _control.WireframeThickness = series.LineWidth;

                var xt = series.Select(m => m.X);

                if (_isLogx)
                {
                    xt = xt.Select(m => Math.Log(m));
                }

                var x = xt.ToArray();

                var yt = series.Select(m => m.Y);

                if (_isLogy)
                {
                    yt = yt.Select(m => Math.Log(m));
                }

                var y = yt.ToArray();

                var zt = series.Select(m => m.Z);

                if (_isLogz)
                {
                    zt = zt.Select(m => Math.Log(m));
                }

                var z = zt.ToArray();

                for (var j = 2; j < series.Count; j += 2)
                {
                    var x1 = x[j - 2];
                    var x2 = x[j - 1];
                    var x3 = x[j - 0];
                    var y1 = y[j - 2];
                    var y2 = y[j - 1];
                    var y3 = y[j - 0];
                    var z1 = z[j - 2];
                    var z2 = z[j - 1];
                    var z3 = z[j - 0];
                    _control.AddWireframeVertex(x1, y1, z1, x2, y2, z2, x3, y3, z3);
                }
            }

            _control.SetTransformWireframe(_plot.MinX, _plot.MaxX, _plot.MinY, _plot.MaxY, _plot.MinZ, _plot.MaxZ);
        }

        public override void RefreshSeries()
        {
            RefreshData();
        }

        public override void RefreshProperties()
        {
            _control.Title = _plot.Title;
            _control.ShowAxis = _plot.Gridlines;

            if (_isLogx != _plot.IsLogX || _isLogy != _plot.IsLogY || _isLogz != _plot.IsLogZ)
            {
                RefreshData();
            }
        }

        // SURFACE:

        public override void RefreshData()
        {
            for (var i = 0; i < _plot.Count; i++)
            {
                _control.SetVertex(i, (Single)_plot[0, i].X, (Single)_plot[0, i].Y, (Single)_plot[0, i].Z);
            }

            RefreshProperties();
        }

        public override void RefreshSeries()
        {
            RefreshProperties();
        }

        public override void RefreshProperties()
        {
            _control.Title = _plot.Title;
            _control.WireframeColor = _plot[0].Color.ColorFromString();
            _control.SetView((Single)_plot.MinX, (Single)_plot.MaxX, (Single)_plot.MinY, (Single)_plot.MaxY, (Single)_plot.MinZ, (Single)_plot.MaxZ);
            _control.SetColors((Single)_plot.MinZ, (Single)_plot.MaxZ, GenerateColors(_plot.ColorPalette, 20));

            _control.Publish();

            _control.SetWireframe(_plot.IsMesh);
            _control.SetSurface(_plot.IsSurf);
            _control.ShowAxis = _plot.Gridlines;
        }
         */
    }
}
