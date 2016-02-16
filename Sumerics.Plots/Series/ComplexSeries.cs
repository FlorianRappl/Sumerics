namespace Sumerics.Plots
{
    using OxyPlot;
    using OxyPlot.Series;
    using System;
    using YAMP;

	sealed class ComplexSeries : XYAxisSeries
	{
		#region Fields

		readonly Func<ScalarValue, ScalarValue> _f;
		readonly BufferState _buffer;
		//OxyBitmap points;

		#endregion

		#region ctor

		public ComplexSeries(Func<ScalarValue, ScalarValue> fz)
		{
			_f = fz;
			_buffer = new BufferState();
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

			if (!state.Equals(_buffer))
			{
                var g = Math.Max(1, (Int32)Math.Sqrt(state.Width * state.Height / 50000.0));
                var width = (Int32)state.Width / g;
                var height = (Int32)state.Height / g;
                var scaleX = (XAxis.ScreenMax.X - XAxis.ScreenMin.X) / width;
                var scaleY = (YAxis.ScreenMax.Y - YAxis.ScreenMin.Y) / height;
                //points = new OxyBitmap(width, height);
                _buffer.ReplaceWith(state);

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
                        var value = _f(z);
						//points.SetPixel(i, j, value.GetColor());
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
	}
}
