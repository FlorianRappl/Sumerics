namespace Sumerics.Controls
{
    using OxyPlot;
    using OxyPlot.Series;
    using System;
    using YAMP;

	class ComplexSeries : XYAxisSeries
	{
		#region Constants

		const double TwoPI = 2.0 * Math.PI;

		#endregion

		#region Fields

		Func<ScalarValue, ScalarValue> f;
		BufferState buffer;
		//OxyBitmap points;

		#endregion

		#region ctor

		public ComplexSeries(Func<ScalarValue, ScalarValue> fz)
		{
			f = fz;
			buffer = new BufferState();
		}

		#endregion

		#region Methods

		public override TrackerHitResult GetNearestPoint(ScreenPoint point, Boolean interpolate)
		{
			return null;
		}

		protected override Boolean IsValidPoint(DataPoint pt)
		{
			return false;
		}

        public override void Render(IRenderContext rc)
        {
			var origin = XAxis.InverseTransform(XAxis.ScreenMin.X, YAxis.ScreenMin.Y, YAxis);
			var end = XAxis.InverseTransform(XAxis.ScreenMax.X, YAxis.ScreenMax.Y, YAxis);

			var state = new BufferState
			{
				MinX = origin.X,
				MaxX = end.X,
				MinY = origin.Y,
				MaxY = end.Y,
                Width = XAxis.ScreenMax.X - XAxis.ScreenMin.X,
                Height = YAxis.ScreenMax.Y - YAxis.ScreenMin.Y
			};

			if (!state.Equals(buffer))
			{
                var g = Math.Max(1, (int)Math.Sqrt(state.Width * state.Height / 50000.0));
                var width = (int)state.Width / g;
                var height = (int)state.Height / g;
                var scaleX = (XAxis.ScreenMax.X - XAxis.ScreenMin.X) / width;
                var scaleY = (YAxis.ScreenMax.Y - YAxis.ScreenMin.Y) / height;
                //points = new OxyBitmap(width, height);
                buffer = state;

				var dx = (end.X - origin.X) / width;
				var dy = (end.Y - origin.Y) / height;
				var sx = XAxis.ScreenMin.X;
				var z = new ScalarValue(origin.X, 0.0);

                for (var i = 0; i < width; i++)
				{
					var sy = YAxis.ScreenMin.Y;
					z.ImaginaryValue = origin.Y;

                    for (var j = 0; j < height; j++)
					{
                        var value = f(z);
						//points.SetPixel(i, j, GetColor(value));
						sy += scaleY;
						z.ImaginaryValue += dy;
					}

					z.Value += dx;
					sx += scaleX;
				}
			}

            //rc.DrawImage(new OxyRect(XAxis.ScreenMin.X, YAxis.ScreenMin.Y, state.Width,state.Height), points);
		}

		#endregion

		#region Color Mapping

		static OxyColor GetColor(ScalarValue fz)
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

        static ColorTriplet ComplexToHsv(Double re, Double im)
		{
			// extract a phase 0 <= t < 2 pi
			var t = Math.Atan2(im, re);

			if (t < 0.0)
				t += TwoPI;

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
                var i = (int)z;
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

		#endregion

		#region Nested Classes

		struct ColorTriplet
		{
			public double X { get; set; }

			public double Y { get; set; }

			public double Z { get; set; }
		}

		class BufferState
		{
			public double MinX { get; set; }

			public double MaxX { get; set; }

			public double MinY { get; set; }

            public double MaxY { get; set; }

            public double Width { get; set; }

            public double Height { get; set; }

			public override bool Equals(object obj)
			{
				if (obj is BufferState)
				{
					var buffer = (BufferState)obj;

					if (buffer.MinY != MinY)
						return false;

					if (buffer.MaxY != MaxY)
						return false;

					if (buffer.MinX != MinX)
						return false;

					if (buffer.MaxX != MaxX)
                        return false;

                    if (buffer.Width != Width)
                        return false;

                    if (buffer.Height != Height)
                        return false;

					return true;
				}

				return false;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		#endregion
	}
}
