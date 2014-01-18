﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeometryRenderContext.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Rendering WPF shapes to a Canvas
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Rendering WPF shapes to a Canvas
    /// </summary>
    public class GeometryRenderContext : IRenderContext
    {
        private GeometryGroup group;

        private readonly Canvas canvas;

        private void Add(Geometry g)
        {
            group.Children.Add(g);
        }

        public GeometryRenderContext(Canvas canvas)
        {
            this.canvas = canvas;

            Width = canvas.ActualWidth;
            Height = canvas.ActualHeight;
        }

        public double Width { get; private set; }

        public double Height { get; private set; }

        Dictionary<OxyColor, Brush> brushCache = new Dictionary<OxyColor, Brush>();

        public void DrawLines(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                             OxyPenLineJoin lineJoin, bool aliased)
        {
            var startPoint = new ScreenPoint();
            bool first = true;
            foreach (var p in points)
            {
                if (!first)
                {
                    Add(new Line { X1 = startPoint.X, Y1 = startPoint.Y, X2 = p.X, Y2 = p.Y });
                }
                else
                {
                    startPoint = p;
                }
                first = !first;
            }
        }

        public void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                             OxyPenLineJoin lineJoin, bool aliased)
        {
            var e = new Polyline();
            if (stroke != null && thickness > 0)
            {
                e.Stroke = GetCachedBrush(stroke);

                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        e.StrokeLineJoin = PenLineJoin.Round;
                        break;
                    case OxyPenLineJoin.Bevel:
                        e.StrokeLineJoin = PenLineJoin.Bevel;
                        break;
                    //  The default StrokeLineJoin is Miter
                }

                if (thickness != 1) // default values is 1
                    e.StrokeThickness = thickness;
                if (dashArray != null)
                    e.StrokeDashArray = new DoubleCollection(dashArray);
            }
            // pl.Fill = null;
            if (aliased)
                e.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            var pc = new PointCollection(points.Count);
            foreach (var p in points)
                pc.Add(ToPoint(p));
            e.Points = pc;

            Add(e);
        }

        private Brush GetCachedBrush(OxyColor stroke)
        {
            Brush brush;
            if (!brushCache.TryGetValue(stroke, out brush))
            {
                brush = new SolidColorBrush(stroke.ToColor());
                brush.Freeze();
                brushCache.Add(stroke, brush);
            }
            return brush;
        }

        public void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness,
                                double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            var e = new Polygon();
            if (stroke != null && thickness > 0)
            {
                e.Stroke = GetCachedBrush(stroke);
                if (thickness != 1)
                    e.StrokeThickness = thickness;
                if (dashArray != null)
                    e.StrokeDashArray = new DoubleCollection(dashArray);
                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        e.StrokeLineJoin = PenLineJoin.Round;
                        break;
                    case OxyPenLineJoin.Bevel:
                        e.StrokeLineJoin = PenLineJoin.Bevel;
                        break;
                    //  The default StrokeLineJoin is Miter
                }
            }
            if (aliased)
                e.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            if (fill != null)
                e.Fill = GetCachedBrush(fill);

            var pc = new PointCollection(points.Count);
            foreach (var p in points)
                pc.Add(ToPoint(p));
            e.Points = pc;

            Add(e);
        }

        ///<summary>
        /// Draws a rectangle.
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="width"></param>
        ///<param name="height"></param>
        ///<param name="fill"></param>
        ///<param name="stroke"></param>
        ///<param name="thickness"></param>
        public void DrawRectangle(double x, double y, double width, double height, OxyColor fill, OxyColor stroke,
                                double thickness)
        {
            var el = new Rectangle();
            if (stroke != null)
            {
                el.Stroke = new SolidColorBrush(stroke.ToColor());
                el.StrokeThickness = thickness;
            }
            if (fill != null)
            {
                el.Fill = new SolidColorBrush(fill.ToColor());
            }

            el.Width = width;
            el.Height = height;
            Canvas.SetLeft(el, x);
            Canvas.SetTop(el, y);
            Add(el);
        }

        public void DrawEllipse(double x, double y, double width, double height, OxyColor fill, OxyColor stroke,
                                double thickness)
        {
            var el = new Ellipse();
            if (stroke != null)
            {
                el.Stroke = new SolidColorBrush(stroke.ToColor());
                el.StrokeThickness = thickness;
            }
            if (fill != null)
            {
                el.Fill = new SolidColorBrush(fill.ToColor());
            }

            el.Width = width;
            el.Height = height;
            Canvas.SetLeft(el, x);
            Canvas.SetTop(el, y);
            Add(el);
        }

        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize,
                             double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            var tb = new TextBlock
                         {
                             Text = text,
                             Foreground = new SolidColorBrush(fill.ToColor())
                         };
            if (fontFamily != null)
                tb.FontFamily = new FontFamily(fontFamily);
            if (fontSize > 0)
                tb.FontSize = fontSize;
            if (fontWeight > 0)
                tb.FontWeight = FontWeight.FromOpenTypeWeight((int)fontWeight);

            tb.Measure(new Size(1000, 1000));

            double dx = 0;
            if (halign == HorizontalTextAlign.Center)
                dx = -tb.DesiredSize.Width / 2;
            if (halign == HorizontalTextAlign.Right)
                dx = -tb.DesiredSize.Width;

            double dy = 0;
            if (valign == VerticalTextAlign.Middle)
                dy = -tb.DesiredSize.Height / 2;
            if (valign == VerticalTextAlign.Bottom)
                dy = -tb.DesiredSize.Height;

            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform(dx, dy));
            if (rotate != 0)
                transform.Children.Add(new RotateTransform(rotate));
            transform.Children.Add(new TranslateTransform(p.X, p.Y));
            tb.RenderTransform = transform;

            tb.SetValue(RenderOptions.ClearTypeHintProperty, ClearTypeHint.Enabled);

            Add(tb);
        }

        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (String.IsNullOrEmpty(text))
                return OxySize.Empty;

            var tb = new TextBlock
                         {
                             Text = text
                         };

            if (fontFamily != null)
                tb.FontFamily = new FontFamily(fontFamily);
            if (fontSize > 0)
                tb.FontSize = fontSize;
            if (fontWeight > 0)
                tb.FontWeight = FontWeight.FromOpenTypeWeight((int)fontWeight);

            tb.Measure(new Size(1000, 1000));

            return new OxySize(tb.DesiredSize.Width, tb.DesiredSize.Height);
        }

        private static Point ToPoint(ScreenPoint point)
        {
            return new Point(point.X, point.Y);
        }
    }
}