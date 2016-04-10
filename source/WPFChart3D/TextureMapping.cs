namespace WPFChart3D
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Media.Media3D;

    public class TextureMapping
    {
        public DiffuseMaterial Material;
        Boolean _bPseudoColor = false;

        public TextureMapping()
        {
            SetRGBMaping();
        }

        public void SetRGBMaping()
        {
            var writeableBitmap = new WriteableBitmap(64, 64, 96, 96, PixelFormats.Bgr24, null);
            writeableBitmap.Lock();

            unsafe
            {
                // Get a pointer to the back buffer.
                var pStart = (byte*)(void*)writeableBitmap.BackBuffer;
                var nL = writeableBitmap.BackBufferStride;

                for (var r = 0; r < 16; r++)
                {
                    for (var g = 0; g < 16; g++)
                    {
                        for (var b = 0; b < 16; b++)
                        {
                            var nX = (g % 4) * 16 + b;
                            var nY = r * 4 + (Int32)(g / 4);

                            *(pStart + nY * nL + nX * 3 + 0) = (Byte)(b * 17);
                            *(pStart + nY * nL + nX * 3 + 1) = (Byte)(g * 17);
                            *(pStart + nY * nL + nX * 3 + 2) = (Byte)(r * 17);
                        }
                    }
                }

            }
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 64, 64));

            // Release the back buffer and make it available for display.
            writeableBitmap.Unlock();

            ImageBrush imageBrush = new ImageBrush(writeableBitmap);
            //ImageBrush imageBrush = new ImageBrush(imSrc);
            imageBrush.ViewportUnits = BrushMappingMode.Absolute;
            Material = new DiffuseMaterial();
            Material.Brush = imageBrush;

            _bPseudoColor = false;
        }

        public void SetPseudoMaping()
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(64, 64, 96, 96, PixelFormats.Bgr24, null);
            writeableBitmap.Lock();

            unsafe
            {
                // Get a pointer to the back buffer.
                var pStart = (byte*)(void*)writeableBitmap.BackBuffer;
                var nL = writeableBitmap.BackBufferStride;

                for (var nY = 0; nY < 64; nY++)
                {
                    for (var nX = 0; nX < 64; nX++)
                    {
                        var nI = nY * 64 + nX;
                        var k = nI / 4095.0;

                        Color color = PseudoColor(k);

                        *(pStart + nY * nL + nX * 3 + 0) = (Byte)(color.B);
                        *(pStart + nY * nL + nX * 3 + 1) = (Byte)(color.G);
                        *(pStart + nY * nL + nX * 3 + 2) = (Byte)(color.R);
                    }
                }
            }

            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 64, 64));

            // Release the back buffer and make it available for display.
            writeableBitmap.Unlock();

            var imageBrush = new ImageBrush(writeableBitmap);
            imageBrush.ViewportUnits = BrushMappingMode.Absolute;
            Material = new DiffuseMaterial();
            Material.Brush = imageBrush;
            _bPseudoColor = true;
        }

        public Point GetMappingPosition(Color color)
        {
            return GetMappingPosition(color, _bPseudoColor);
        }

        public static Point GetMappingPosition(Color color, Boolean bPseudoColor)
        {
            if (bPseudoColor)
            {
                var r = color.R / 255.0;
                var g = color.G / 255.0;
                var b = color.B / 255.0;

                var k = 0.0;

                if ((b >= g) && (b > r))
                {
                    k = 0.25 * g;
                }
                else if ((g > b) && (b >= r))
                {
                    k = 0.25 + 0.25 * (1 - b);
                }
                else if ((g >= r) && (r > b))
                {
                    k = 0.5 + 0.25 * r;
                }
                else
                {
                    k = 0.75 + 0.25 * (1 - g);
                }

                var nI = Math.Max(Math.Min((Int32)(k * 4095), 4095), 0);
                var nY = nI / 64;
                var nX = nI % 64;
                var x1 = (Double)nX;
                var y1 = (Double)nY;
                return new Point(x1 / 64, y1 / 64);
            }
            else
            {
                var nR = (color.R) / 17;
                var nG = (color.G) / 17;
                var nB = (color.B) / 17;
                var nX = (nG % 4) * 16 + nB;
                var nY = nR * 4 + nG / 4;
                var x1 = (Double)nX;
                var y1 = (Double)nY;
                return new Point(x1 / 63, y1 / 63);
            }
        }

        static public Color PseudoColor(Double k)
        {
            k = Math.Max(Math.Min(k, 1), 0);

            var r = 0.0;
            var g = 0.0;
            var b = 0.0;

            if (k < 0.25)
            {
                r = 0;
                g = 4 * k;
                b = 1;
            }
            else if (k < 0.5)
            {
                r = 0;
                g = 1;
                b = 1 - 4 * (k - 0.25);
            }
            else if (k < 0.75)
            {
                r = 4 * (k - 0.5);
                g = 1;
                b = 0;
            }
            else
            {
                r = 1;
                g = 1 - 4 * (k - 0.75);
                b = 0;
            }

            var R = (Byte)(r * 255 + 0.0);
            var G = (Byte)(g * 255 + 0.0);
            var B = (Byte)(b * 255 + 0.0);

            return Color.FromRgb(R, G, B);
        }
    }
}
