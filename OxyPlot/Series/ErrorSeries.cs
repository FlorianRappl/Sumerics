using System;
using System.Collections.Generic;
using OxyPlot;

namespace Sumerics.Controls
{
	/// <summary>
	/// Represents an error series.
	/// </summary>
	public class ErrorSeries : LineSeries
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorSeries"/> class.
		/// </summary>
		public ErrorSeries()
		{
			this.Color = OxyColors.Black;
			this.StrokeThickness = 1;
		}

		/// <summary>
		/// Renders the series on the specified render context.
		/// </summary>
		/// <param name="rc">The rendering context.</param>
		/// <param name="model">The model.</param>
		public override void Render(IRenderContext rc, PlotModel model)
		{
			base.Render(rc, model);
			var points = this.Points;

			if (points.Count == 0)
				return;

			if (this.XAxis == null || this.YAxis == null)
			{
				Trace("Axis not defined.");
				return;
			}

			var clippingRect = GetClippingRect();
			int n = points.Count;

			// Transform all points to screen coordinates
			var segments = new List<ScreenPoint>(n * 6);

			for (int i = 0; i < n; i++)
			{
				var sp = XAxis.Transform(points[i].X, points[i].Y, YAxis);
				var ei = points[i] as ErrorItem;
				var errorx = ei != null ? ei.XError * XAxis.Scale : 0.0;
				var errory = ei != null ? ei.YError * Math.Abs(YAxis.Scale) : 0.0;
				var d = 4.0;

				if (errorx > 0)
				{
					var p0 = new ScreenPoint(sp.X - (errorx * 0.5), sp.Y);
					var p1 = new ScreenPoint(sp.X + (errorx * 0.5), sp.Y);
					segments.Add(p0);
					segments.Add(p1);
					segments.Add(new ScreenPoint(p0.X, p0.Y - d));
					segments.Add(new ScreenPoint(p0.X, p0.Y + d));
					segments.Add(new ScreenPoint(p1.X, p1.Y - d));
					segments.Add(new ScreenPoint(p1.X, p1.Y + d));
				}

				if (errory > 0)
				{
					var p0 = new ScreenPoint(sp.X, sp.Y - (errory * 0.5));
					var p1 = new ScreenPoint(sp.X, sp.Y + (errory * 0.5));
					segments.Add(p0);
					segments.Add(p1);
					segments.Add(new ScreenPoint(p0.X - d, p0.Y));
					segments.Add(new ScreenPoint(p0.X + d, p0.Y));
					segments.Add(new ScreenPoint(p1.X - d, p1.Y));
					segments.Add(new ScreenPoint(p1.X + d, p1.Y));
				}
			}

			// clip the line segments with the clipping rectangle
			for (int i = 0; i + 1 < segments.Count; i += 2)
			{
				rc.DrawClippedLine(
					new[] { segments[i], segments[i + 1] },
					clippingRect,
					2,
					GetSelectableColor(Color),
					1.0,
					LineStyle.Solid,
					OxyPenLineJoin.Bevel,
					true);
			}
		}
	}
}