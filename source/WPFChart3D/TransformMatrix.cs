namespace WPFChart3D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Media3D;

    public class TransformMatrix
    {
        const Double DegInverse = 1.0 / 180.0;
        const Double ScaleFactor = 1.3;

        Matrix3D _viewMatrix = Matrix3D.Identity;
        Matrix3D _projMatrix = Matrix3D.Identity;
        Boolean _mouseDown = false;
        Point _movePoint;

        public Matrix3D TotalMatrix
        {
            get { return Matrix3D.Multiply(_projMatrix, _viewMatrix); }
        }

        public void OnLBtnDown(Point pt)
        {
            _mouseDown = true;
            _movePoint = pt;
        }

        public void OnMouseMove(Point pt, System.Windows.Controls.Viewport3D viewPort)
        {
            if (_mouseDown)
            {
                var viewMatrix = new Matrix3D();
                var width = viewPort.ActualWidth;
                var height = viewPort.ActualHeight;
                viewMatrix.Append(_viewMatrix);

                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                }
                else if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    var shiftX = 2 * (pt.X - _movePoint.X) / (width);
                    var shiftY = -2 * (pt.Y - _movePoint.Y) / (width);

                    viewMatrix.Translate(new Vector3D(shiftX, shiftY, 0));
                    _movePoint = pt;
                }
                else
                {
                    var aY = 180 * (pt.X - _movePoint.X) / width;
                    var aX = 180 * (pt.Y - _movePoint.Y) / height;

                    viewMatrix.Rotate(new Quaternion(new Vector3D(1, 0, 0), aX));
                    viewMatrix.Rotate(new Quaternion(new Vector3D(0, 1, 0), aY));
                    _movePoint = pt;
                }

                _viewMatrix = viewMatrix;
            }
        }
		
        public void OnLBtnUp()
        {
            _mouseDown = false;
        }

        public void OnKeyDown(KeyEventArgs args)
        {
            var f = 1.0 / ScaleFactor;
            var viewMatrix = new Matrix3D();
            viewMatrix.Append(_viewMatrix);

            switch (args.Key)
            {
                case Key.Home:
                    viewMatrix = Matrix3D.Identity;
                    break;

                case Key.OemPlus:
                    viewMatrix.Scale(new Vector3D(f, f, f));
                    break;

                case Key.OemMinus:
                    viewMatrix.Scale(new Vector3D(f, f, f));
                    break;

                default:
                    return;
            }

            _viewMatrix = viewMatrix;
        }

        public static void Transform(Mesh3D model, Point3D center, Double aX, Double aZ)
        {
            var angleX = 3.1415926 * aX * DegInverse;
            var angleZ = 3.1415926 * aZ * DegInverse;
            var nVertNo = model.GetVertexNo();

            for (var i = 0; i < nVertNo; i++)
            {
                var pt1 = model.GetPoint(i);

                // rotate from z-axis
                var x2 = pt1.X * Math.Cos(angleZ) + pt1.Z * Math.Sin(angleZ);
                var y2 = pt1.Y;
                var z2 = -pt1.X * Math.Sin(angleZ) + pt1.Z * Math.Cos(angleZ);

                var x3 = center.X + x2 * Math.Cos(angleX) - y2 * Math.Sin(angleX);
                var y3 = center.Y + x2 * Math.Sin(angleX) + y2 * Math.Cos(angleX);
                var z3 = center.Z + z2;

                model.SetPoint(i, x3, y3, z3);
            }
        }

        public void CalculateProjectionMatrix(Double xMin, Double xMax, Double yMin, Double yMax, Double zMin, Double zMax, Double scaleFactor)
        {
            var projMatrix = Matrix3D.Identity;
            var xC = (xMin + xMax) * 0.5;
            var yC = (yMin + yMax) * 0.5;
            var zC = (zMin + zMax) * 0.5;

            var xRange = (xMax - xMin) * 0.5;
            var yRange = (yMax - yMin) * 0.5;
            var zRange = (zMax - zMin) * 0.5;

            var v = new Vector3D(-xC, -yC, -zC);
            projMatrix.Translate(v);

            if (xRange >= 1e-10)
            {
                var sX = scaleFactor / xRange;
                var sY = scaleFactor / yRange;
                var sZ = scaleFactor / zRange;

                var s = new Vector3D(sX, sY, sZ);

                projMatrix.Scale(s);
            }

            _projMatrix = projMatrix;
        }

        public Point VertexToScreenPt(Point3D point, Viewport3D viewPort)
        {
            var pt2 = TotalMatrix.Transform(point);

            var width = viewPort.ActualWidth;
            var height = viewPort.ActualHeight;

            var x3 = width * 0.5 + (pt2.X) * width * 0.5;
            var y3 = height * 0.5 - (pt2.Y) * width * 0.5;

            return new Point(x3, y3);
        }

        public static Point ScreenPtToViewportPt(Point point, Viewport3D viewPort)
        {
            var width = viewPort.ActualWidth;
            var height = viewPort.ActualHeight;
            var wi = 1.0 / width;

            var x3 = (Double)point.X;
            var y3 = (Double)point.Y;

            var x2 = (x3 - width * 0.5) * 2 * wi;
            var y2 = (height * 0.5 - y3) * 2 * wi;

            return new Point(x2, y2);
        }

        public Point VertexToViewportPt(Point3D point, Viewport3D viewPort)
        {
            var pt2 = TotalMatrix.Transform(point);
            return new Point(pt2.X, pt2.Y);
        }
    }
}
