using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMP;

namespace Sumerics.Controls
{
    class HeatmapSeries : XYAxisSeries
    {
		#region Members

		OxyBitmap buffer;
        OxyColor[] colors;
        double spacing;
        Points<HeatmapPlotValue.HeatPoint> series;

        int lastXs;
        int lastYs;
        int lastXe;
        int lastYe;

        int width;
        int height;

        /// <summary>
        /// Set a maximum size - when to prefer using bitmaps?!
        /// </summary>
        const int MAXIMUM_ENTRIES = 45 * 45;

		#endregion

		#region ctor

        public HeatmapSeries(int width, int height, Points<HeatmapPlotValue.HeatPoint> heatpoints)
		{
            series = heatpoints;
            this.width = width;
            this.height = height;
		}

		#endregion

        #region Properties

        public bool IsInterpolated
        {
            get;
            set;
        }

        public OxyColor[] HeatmapColors
        {
            get { return colors; }
            set
            {
                colors = value;
                buffer = null;
                spacing = 1.0 / value.Length;
            }
        }

        #endregion

        #region Methods

        int GetIndex(double p)
        {
            return Math.Min((int)Math.Floor(p / spacing), HeatmapColors.Length - 1);
        }

		public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
		{
			return null;
		}

		protected override bool IsValidPoint(IDataPoint pt, Axis xaxis, Axis yaxis)
		{
			return false;
		}

		public override void Render(IRenderContext rc, PlotModel model)
		{
            if (HeatmapColors == null)
                return;

            //In a low case we just draw rectangles
            if (series.Count < MAXIMUM_ENTRIES && !IsInterpolated)
            {
                var left = XAxis.ScreenMin.X;
                var right = XAxis.ScreenMax.X;
                var bottom = YAxis.ScreenMin.Y;
                var top = YAxis.ScreenMax.Y;

                var width = Math.Abs(XAxis.Scale);
                var height = Math.Abs(YAxis.Scale);

                foreach (var point in series)
                {
                    var sp = XAxis.Transform(point.Column - 1, point.Row - 1, YAxis);

                    if (sp.X > right || sp.X + width < left || sp.Y > bottom || sp.Y + height < top)
                        continue;

                    rc.DrawRectangle(new OxyRect
                    {
                        Left = Math.Max(sp.X, left),
                        Top = Math.Max(sp.Y, top),
                        Width = Math.Min(Math.Min(width + sp.X, right) - sp.X, sp.X - left + width),
                        Height = Math.Min(Math.Min(height + sp.Y, bottom) - sp.Y, sp.Y - top + height)
                    }, HeatmapColors[GetIndex(point.Magnitude)], null);
                }
            }
            else //otherwise we use an image for better performance
            {
                var rect = new OxyRect
                {
                    Left = XAxis.ScreenMin.X,
                    Top = Math.Min(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y),
                    Width = Math.Abs(XAxis.ScreenMax.X - XAxis.ScreenMin.X),
                    Height = Math.Abs(YAxis.ScreenMax.Y - YAxis.ScreenMin.Y)
                };

                var xs = (int)Math.Floor(XAxis.ActualMinimum);
                var ys = (int)Math.Floor(YAxis.ActualMinimum);
                var xe = (int)Math.Ceiling(XAxis.ActualMaximum);
                var ye = (int)Math.Ceiling(YAxis.ActualMaximum);

                if (buffer == null || IsDifferent(xs, ys, xe, ye))
                {
                    buffer = new OxyBitmap(xe - xs, ye - ys);
                    var sx = xs + 1;
                    var sy = ys + 1;

                    foreach (var point in series)
                    {
                        if (point.Column > xe || point.Column < sx || point.Row > ye || point.Row < sy)
                            continue;

                        buffer.SetPixel(point.Column - sx, point.Row - sy, HeatmapColors[GetIndex(point.Magnitude)]);
                    }
                }

                buffer.Interpolation = IsInterpolated;
                rc.DrawImage(rect, buffer);
            }
		}

        bool IsDifferent(int xs, int ys, int xe, int ye)
        {
            if (xs != lastXs || ys != lastYs || xe != lastXe || ye != lastYe)
            {
                lastXs = xs;
                lastYs = ys;
                lastXe = xe;
                lastYe = ye;
                return true;
            }

            return false;
        }

		#endregion
    }
}
