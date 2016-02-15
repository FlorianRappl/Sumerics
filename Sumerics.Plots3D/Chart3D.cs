namespace WPFChart3D
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    class Chart3D
    {
        public static Int32 ShapeNumber = 5;

        protected Vertex3D[] _vertices;
        protected Single _xMin;
        protected Single _xMax;
        protected Single _yMin;
        protected Single _yMax;
        protected Single _zMin;
        protected Single _zMax;

        Single _axisLengthWidthRatio = 200;
        Single _xAxisLength;
        Single _yAxisLength;
        Single _zAxisLength;
        Single _xAxisCenter;
        Single _yAxisCenter;
        Single _zAxisCenter;
        Boolean _bUseAxes = false;

        public Color _axisColor = Color.FromRgb(0, 0, 196);

        public Vertex3D this[Int32 n]
        {
            get { return _vertices[n]; }
            set { _vertices[n] = value; }
        }

        public Single XCenter()
        {
            return (_xMin + _xMax) / 2;
        }

        public Single YCenter()
        {
            return (_yMin + _yMax) / 2;
        }

        public Single XRange()
        {
            return _xMax - _xMin;
        }

        public Single YRange()
        {
            return _yMax - _yMin;
        }

        public Single ZRange()
        {
            return _zMax - _zMin;
        }

        public Single XMin()
        {
            return _xMin;
        }

        public Single XMax()
        {
            return _xMax;
        }

        public Single YMin()
        {
            return _yMin;
        }

        public Single YMax()
        {
            return _yMax;
        }

        public Single ZMin()
        {
            return _zMin;
        }

        public Single ZMax()
        {
            return _zMax;
        }

        public Int32 GetDataNo()
        {
            return _vertices.Length;
        }

        public void SetDataNo(Int32 nSize)
        {
            _vertices = new Vertex3D[nSize];
        }

        public void SetDataRange(Single xmin, Single xmax, Single ymin, Single ymax, Single zmin, Single zmax)
        {
            _xMin = xmin;
            _yMin = ymin;
            _zMin = zmin;
            _xMax = xmax;
            _yMax = ymax;
            _zMax = zmax;
        }

        public void GetDataRange()
        {
            var nDataNo = GetDataNo();

            if (nDataNo != 0)
            {
                _xMin = Single.MaxValue;
                _yMin = Single.MaxValue;
                _zMin = Single.MaxValue;
                _xMax = Single.MinValue;
                _yMax = Single.MinValue;
                _zMax = Single.MinValue;

                for (var i = 0; i < nDataNo; i++)
                {
                    var xV = this[i].X;
                    var yV = this[i].Y;
                    var zV = this[i].Z;

                    if (_xMin > xV)
                        _xMin = xV;
                    if (_yMin > yV)
                        _yMin = yV;
                    if (_zMin > zV)
                        _zMin = zV;
                    if (_xMax < xV)
                        _xMax = xV;
                    if (_yMax < yV)
                        _yMax = yV;
                    if (_zMax < zV)
                        _zMax = zV;
                }
            }
        }

        public void SetAxes(Single x0, Single y0, Single z0, Single xL, Single yL, Single zL)
        {
            _xAxisLength = xL;
            _yAxisLength = yL;
            _zAxisLength = zL;
            _xAxisCenter = x0;
            _yAxisCenter = y0;
            _zAxisCenter = z0;
            _bUseAxes = true;
        }

        public void SetAxes()
        {
            SetAxes(0.05f);
        }

        public void SetAxes(Single margin)
        {
            var xRange = _xMax - _xMin;
            var yRange = _yMax - _yMin;
            var zRange = _zMax - _zMin;

            var xC = _xMin - margin * xRange;
            var yC = _yMin - margin * yRange;
            var zC = _zMin - margin * zRange;
            var xL = (1 + 2 * margin) * xRange;
            var yL = (1 + 2 * margin) * yRange;
            var zL = (1 + 2 * margin) * zRange;

            SetAxes(xC, yC, zC, xL, yL, zL);
        }

		public void AddAxesMeshes(List<Mesh3D> meshs)
        {
            if (_bUseAxes)
            {
                var radius = (_xAxisLength + _yAxisLength + _zAxisLength) / (3 * _axisLengthWidthRatio);

                var xAxisCylinder = new Cylinder3D(radius, radius, _xAxisLength, 6);
                xAxisCylinder.SetColor(_axisColor);
                TransformMatrix.Transform(xAxisCylinder, new Point3D(_xAxisCenter + _xAxisLength / 2, _yAxisCenter, _zAxisCenter), 0, 90);
                meshs.Add(xAxisCylinder);

                var xAxisCone = new Cone3D(2 * radius, 2 * radius, radius * 5, 6);
                xAxisCone.SetColor(_axisColor);
                TransformMatrix.Transform(xAxisCone, new Point3D(_xAxisCenter + _xAxisLength, _yAxisCenter, _zAxisCenter), 0, 90);
                meshs.Add(xAxisCone);

                var yAxisCylinder = new Cylinder3D(radius, radius, _yAxisLength, 6);
                yAxisCylinder.SetColor(_axisColor);
                TransformMatrix.Transform(yAxisCylinder, new Point3D(_xAxisCenter, _yAxisCenter + _yAxisLength / 2, _zAxisCenter), 90, 90);
                meshs.Add(yAxisCylinder);

                var yAxisCone = new Cone3D(2 * radius, 2 * radius, radius * 5, 6);
                yAxisCone.SetColor(_axisColor);
                TransformMatrix.Transform(yAxisCone, new Point3D(_xAxisCenter, _yAxisCenter + _yAxisLength, _zAxisCenter), 90, 90);
                meshs.Add(yAxisCone);

                var zAxisCylinder = new Cylinder3D(radius, radius, _zAxisLength, 6);
                zAxisCylinder.SetColor(_axisColor);
                TransformMatrix.Transform(zAxisCylinder, new Point3D(_xAxisCenter, _yAxisCenter, _zAxisCenter + _zAxisLength / 2), 0, 0);
                meshs.Add(zAxisCylinder);

                var zAxisCone = new Cone3D(2 * radius, 2 * radius, radius * 5, 6);
                zAxisCone.SetColor(_axisColor);
                TransformMatrix.Transform(zAxisCone, new Point3D(_xAxisCenter, _yAxisCenter, _zAxisCenter + _zAxisLength), 0, 0);
                meshs.Add(zAxisCone);
            }
        }

        public virtual void Select(ViewportRect rect, TransformMatrix matrix, Viewport3D viewport3d)
        {
        }

        public virtual void HighlightSelection(MeshGeometry3D meshGeometry, Color selectColor)
        {
        }
    }
}
