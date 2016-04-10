namespace Sumerics.Plots
{
    using OxyPlot;
    using System;
    using System.Globalization;
    using YAMP;

    static class ColorExtensions
    {
        public static OxyColor OxyColorFromString(this String color)
        {
            var value = 0u;

            if (color.Length == 7 && color[0] == '#' && UInt32.TryParse(color.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                return OxyColor.FromUInt32(value);
            }
            else if (color.Length >= 10 && color.StartsWith("rgb(", StringComparison.InvariantCultureIgnoreCase) && color[color.Length - 1] == ')')
            {
                var content = color.Substring(4, color.Length - 5);
                var commas = 0;

                for (var i = 0; i < content.Length; i++)
                {
                    if (content[i] == ',')
                    {
                        commas++;
                    }
                }

                if (commas == 3)
                {
                    content += ",0";
                }

                try 
                {
                    return OxyColor.Parse(content);
                }
                catch 
                {
                }
            }
            else
            {
                var fields = typeof(OxyColors).GetFields();

                foreach (var field in fields)
                {
                    if (field.Name.Equals(color, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return (OxyColor)field.GetValue(null);
                    }
                }
            }

            return OxyColors.Black;
        }

        public static OxyColor GetColor(this ScalarValue fz)
        {
            var re = fz.Re;
            var im = fz.Im;

            if (!Double.IsInfinity(re) && !Double.IsNaN(re) && !Double.IsInfinity(im) && !Double.IsNaN(im))
            {
                // convert the complex function value to a HSV color triplet
                var hsv = ComplexToHsv(re, im);

                // convert the HSV color triplet to an RBG color triplet
                var rgb = HsvToRgb(hsv);
                var r = (Byte)Math.Truncate(255.0 * rgb.X);
                var g = (Byte)Math.Truncate(255.0 * rgb.Y);
                var b = (Byte)Math.Truncate(255.0 * rgb.Z);

                return OxyColor.FromRgb(r, g, b);
            }

            return OxyColors.White;
        }

        public static OxyColor[] GenerateColors(this ColorPalettes palette, Int32 length)
        {
            var colors = new OxyColor[length];
            var args = new Object[] { length };
            var paletteString = palette.ToString();
            var method = typeof(OxyPalettes).GetMethod(paletteString);
            var oxyPalette = default(OxyPalette);

            if (method != null)
            {
                oxyPalette = method.Invoke(null, args) as OxyPalette;
            }

            if (oxyPalette == null)
            {
                oxyPalette = OxyPalettes.Jet(length);
            }

            for (var i = 0; i < length; i++)
            {
                colors[i] = oxyPalette.Colors[i];
            }

            return colors;
        }

        static ColorTriplet ComplexToHsv(Double re, Double im)
        {
            const Double TwoPI = 2.0 * Math.PI;
            // extract a phase 0 <= t < 2 pi
            var t = Math.Atan2(im, re);

            if (t < 0.0)
            {
                t += TwoPI;
            }

            // the hue is determined by the phase
            var h = t / TwoPI;

            // extract a magnitude m >= 0
            var m = Math.Sqrt(re * re + im * im);

            // map the magnitude logrithmicly into the repeating interval 0 < r < 1
            // this is essentially where we are between countour lines
            var r0 = 0.0;
            var r1 = 1.0;

            while (m > r1)
            {
                r0 = r1;
                r1 = r1 * Math.E;
            }

            // this puts contour lines at 0, 1, e, e^2, e^3, ...
            var r = (m - r0) / (r1 - r0);

            // determine saturation and value based on r
            // p and q are complementary distances from a countour line
            var p = r < 0.5 ? 2.0 * r : 2.0 * (1.0 - r);
            var q = 1.0 - p;

            // only let p and q go to zero very close to zero; otherwise they should stay nearly 1
            // this keep the countour lines from getting thick
            var p1 = 1 - q * q * q;
            var q1 = 1 - p * p * p;

            // fix s and v from p1 and q1
            var s = 0.4 + 0.6 * p1;
            var v = 0.6 + 0.4 * q1;

            return new ColorTriplet { X = h, Y = s, Z = v };
        }

        // convert HSV to RGB
        static ColorTriplet HsvToRgb(ColorTriplet hsv)
        {
            var h = hsv.X;
            var s = hsv.Y;
            var v = hsv.Z;

            var r = 0.0;
            var g = 0.0;
            var b = 0.0;

            if (s != 0)
            {
                if (h == 1.0)
                {
                    h = 0.0;
                }

                var z = Math.Truncate(6 * h);
                var i = (Int32)z;
                var f = h * 6 - z;
                var p = v * (1 - s);
                var q = v * (1 - s * f);
                var t = v * (1 - s * (1 - f));

                switch (i)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                return new ColorTriplet { X = r, Y = g, Z = b };
            }

            return new ColorTriplet { X = v, Y = v, Z = v };
        }
    }
}
