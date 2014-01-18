using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxyPlot
{
    /// <summary>
    /// Creates a new bitmap.
    /// </summary>
    public class OxyBitmap
    {
        /// <summary>
        /// Creates a new oxy bitmap.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        public OxyBitmap (int width, int height)
	    {
            Width = width;
            Height = height;
            Pixels = new OxyColor[width, height];

            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    Pixels[i, j] = OxyColors.White;

            Interpolation = true;
	    }

        /// <summary>
        /// Gets or sets if interpolation is active.
        /// </summary>
        public bool Interpolation { get; set; }

        /// <summary>
        /// Gets the width of the image in pixels.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the image in pixels.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the 2D pixel array.
        /// </summary>
        public OxyColor[,] Pixels { get; private set; }

        /// <summary>
        /// Sets a pixel in the bitmap.
        /// </summary>
        /// <param name="x">The 0-based x coordinate.</param>
        /// <param name="y">The 0-based y-coordinate.</param>
        /// <param name="color">The color to use.</param>
        /// <returns>The current bitmap instance.</returns>
        public OxyBitmap SetPixel(int x, int y, OxyColor color)
        {
            Pixels[x, y] = color;
            return this;
        }

        /// <summary>
        /// Sets a pixel in the bitmap.
        /// </summary>
        /// <param name="x">The 0-based x coordinate.</param>
        /// <param name="y">The 0-based y coordinate.</param>
        /// <param name="r">The red color 0-255.</param>
        /// <param name="g">The green color 0-255.</param>
        /// <param name="b">The blue color 0-255.</param>
        /// <returns>The current instance.</returns>
        public OxyBitmap SetPixel(int x, int y, byte r, byte g, byte b)
        {
            Pixels[x, y] = OxyColor.FromRgb(r, g, b);
            return this;
        }

        /// <summary>
        /// Gets the color of a specified pixel.
        /// </summary>
        /// <param name="x">The 0-based x coordinate.</param>
        /// <param name="y">The 0-based y coordinate.</param>
        /// <returns>The color of the pixel.</returns>
        public OxyColor GetPixel(int x, int y)
        {
            return Pixels[x, y];
        }
    }
}
